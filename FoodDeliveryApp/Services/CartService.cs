using System.Collections.Generic;
using FoodDeliveryApp.DataStructures;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services
{
    /// <summary>
    /// Manages the in-memory shopping cart using a <see cref="CustomLinkedList{T}"/>.
    /// Supports adding, removing, and querying cart items.
    /// </summary>
    public static class CartService
    {
        /// <summary>
        /// Internal linked list storing the current cart items.
        /// </summary>
        private static CustomLinkedList<CartItem> _cart = new CustomLinkedList<CartItem>();

        /// <summary>
        /// Adds an item to the cart. If the same product and variation already exist,
        /// increments the quantity instead of adding a duplicate entry.
        /// </summary>
        /// <param name="item">The cart item to add.</param>
        public static void AddToCart(CartItem item)
        {
            // Check if the same product + variation is already in the cart
            foreach (var existing in _cart)
            {
                if (existing.Product.Id == item.Product.Id &&
                    existing.Variation.Id == item.Variation.Id)
                {
                    existing.Quantity += item.Quantity;
                    return;
                }
            }

            // New item — add to the end of the linked list
            _cart.AddLast(item);
        }

        /// <summary>
        /// Removes a cart item from the linked list using value-based equality.
        /// </summary>
        /// <param name="item">The cart item to remove.</param>
        public static void RemoveFromCart(CartItem item)
        {
            _cart.Remove(item);
        }

        /// <summary>
        /// Returns a snapshot of all cart items as a standard <see cref="List{T}"/>.
        /// </summary>
        /// <returns>A list of current <see cref="CartItem"/> objects.</returns>
        public static List<CartItem> GetCartItems()
        {
            return _cart.ToList();
        }

        /// <summary>
        /// Calculates the total price of all items in the cart.
        /// </summary>
        /// <returns>The sum of all item subtotals.</returns>
        public static decimal GetTotal()
        {
            decimal total = 0;
            foreach (var item in _cart)
            {
                total += item.Subtotal;
            }
            return total;
        }

        /// <summary>
        /// Gets the number of distinct items in the cart.
        /// </summary>
        /// <returns>The item count.</returns>
        public static int GetItemCount()
        {
            return _cart.Count;
        }

        /// <summary>
        /// Removes all items from the cart.
        /// </summary>
        public static void ClearCart()
        {
            _cart.Clear();
        }
    }
}
