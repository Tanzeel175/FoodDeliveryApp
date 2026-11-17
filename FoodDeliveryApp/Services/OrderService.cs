using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.DataStructures;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services
{
    /// <summary>
    /// Manages order placement and processing using a <see cref="CustomQueue{T}"/>
    /// as an in-memory FIFO pipeline, with the SQLite database as the source of truth.
    /// </summary>
    public static class OrderService
    {
        /// <summary>
        /// In-memory FIFO queue of pending orders awaiting processing.
        /// </summary>
        private static CustomQueue<Order> _orderQueue = new CustomQueue<Order>();

        /// <summary>
        /// Initializes the order queue by loading all pending orders from the database.
        /// Should be called once at application startup.
        /// </summary>
        public static void Initialize()
        {
            _orderQueue.Clear();

            using (var connection = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT Id, UserId, CustomerName, Items, TotalAmount, PlacedAt, Status " +
                             "FROM Orders WHERE Status = 'Pending' ORDER BY Id";
                using (var cmd = new SQLiteCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var order = new Order
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            CustomerName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Items = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            TotalAmount = (decimal)reader.GetDouble(4),
                            PlacedAt = DateTime.Parse(reader.GetString(5)),
                            Status = reader.GetString(6)
                        };
                        _orderQueue.Enqueue(order);
                    }
                }
            }
        }

        /// <summary>
        /// Places a new order: saves it to the database with Status="Pending",
        /// enqueues it into the in-memory queue, and returns the created order.
        /// </summary>
        /// <param name="user">The user placing the order.</param>
        /// <param name="cartItems">The list of items in the user's cart.</param>
        /// <param name="total">The total price of the order.</param>
        /// <returns>The newly created <see cref="Order"/>.</returns>
        public static Order PlaceOrder(User user, List<CartItem> cartItems, decimal total)
        {
            // Serialize cart items as "ProductName (VariationName) x Quantity" joined by "; "
            string itemsSummary = string.Join("; ",
                cartItems.Select(ci => $"{ci.Product.Name} ({ci.Variation.Name}) x {ci.Quantity}"));

            var order = new Order
            {
                UserId = user.Id,
                CustomerName = user.Username,
                Items = itemsSummary,
                TotalAmount = total,
                PlacedAt = DateTime.Now,
                Status = "Pending"
            };

            // Save to database
            using (var connection = DatabaseHelper.GetConnection())
            {
                string sql = @"INSERT INTO Orders (UserId, CustomerName, Items, TotalAmount, PlacedAt, Status)
                               VALUES (@userId, @customerName, @items, @totalAmount, @placedAt, @status)";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", order.UserId);
                    cmd.Parameters.AddWithValue("@customerName", order.CustomerName);
                    cmd.Parameters.AddWithValue("@items", order.Items);
                    cmd.Parameters.AddWithValue("@totalAmount", (double)order.TotalAmount);
                    cmd.Parameters.AddWithValue("@placedAt", order.PlacedAt.ToString("o"));
                    cmd.Parameters.AddWithValue("@status", order.Status);
                    cmd.ExecuteNonQuery();
                }

                // Retrieve the auto-generated Id
                using (var cmd = new SQLiteCommand("SELECT last_insert_rowid()", connection))
                {
                    order.Id = Convert.ToInt32((long)cmd.ExecuteScalar());
                }
            }

            // Enqueue into the in-memory FIFO pipeline
            _orderQueue.Enqueue(order);

            return order;
        }

        /// <summary>
        /// Returns a snapshot of all pending orders currently in the queue.
        /// </summary>
        /// <returns>A list of pending <see cref="Order"/> objects in FIFO order.</returns>
        public static List<Order> GetPendingOrders()
        {
            return _orderQueue.ToList();
        }

        /// <summary>
        /// Dequeues the front (oldest) pending order, marks it as "Completed" in the database,
        /// and returns it. Returns null if the queue is empty.
        /// </summary>
        /// <returns>The processed <see cref="Order"/>, or <c>null</c> if no pending orders exist.</returns>
        public static Order ProcessNextOrder()
        {
            if (_orderQueue.IsEmpty)
                return null;

            var order = _orderQueue.Dequeue();
            order.Status = "Completed";

            // Update the database
            using (var connection = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE Orders SET Status = @status WHERE Id = @id";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@status", order.Status);
                    cmd.Parameters.AddWithValue("@id", order.Id);
                    cmd.ExecuteNonQuery();
                }
            }

            return order;
        }

        /// <summary>
        /// Gets the number of pending orders currently in the queue.
        /// </summary>
        /// <returns>The count of pending orders.</returns>
        public static int GetPendingCount()
        {
            return _orderQueue.Count;
        }

        /// <summary>
        /// Loads the complete order history (all statuses) from the database.
        /// </summary>
        /// <returns>A list of all <see cref="Order"/> objects, ordered by most recent first.</returns>
        public static List<Order> GetOrderHistory()
        {
            var orders = new List<Order>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT Id, UserId, CustomerName, Items, TotalAmount, PlacedAt, Status " +
                             "FROM Orders ORDER BY Id DESC";
                using (var cmd = new SQLiteCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            CustomerName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Items = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            TotalAmount = (decimal)reader.GetDouble(4),
                            PlacedAt = DateTime.Parse(reader.GetString(5)),
                            Status = reader.GetString(6)
                        });
                    }
                }
            }

            return orders;
        }
    }
}
