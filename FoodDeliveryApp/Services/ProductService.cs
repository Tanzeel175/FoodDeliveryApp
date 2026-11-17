using System.Collections.Generic;
using System.Data.SQLite;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services
{
    /// <summary>
    /// Provides read access to the product catalog from the database.
    /// </summary>
    public static class ProductService
    {
        /// <summary>
        /// Loads all products from the database, including their associated variations.
        /// </summary>
        /// <returns>A list of all <see cref="Product"/> objects with populated <see cref="Product.Variations"/>.</returns>
        public static List<string> GetCategories()
        {
            var categories = new List<string>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT DISTINCT Category FROM Products ORDER BY Category";
                using (var cmd = new SQLiteCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(reader.GetString(0));
                    }
                }
            }
            return categories;
        }

        public static List<Product> GetProductsByCategory(string category)
        {
            var products = new Dictionary<int, Product>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                string productSql = "SELECT Id, Category, Name, Description, ImagePath FROM Products WHERE Category = @category";
                using (var cmd = new SQLiteCommand(productSql, connection))
                {
                    cmd.Parameters.AddWithValue("@category", category);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                Id = reader.GetInt32(0),
                                Category = reader.GetString(1),
                                Name = reader.GetString(2),
                                Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                ImagePath = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Variations = new List<ProductVariation>()
                            };
                            products[product.Id] = product;
                        }
                    }
                }

                if (products.Count == 0) return new List<Product>();

                string productIds = string.Join(",", products.Keys);
                string variationSql = $"SELECT Id, ProductId, Name, Price FROM ProductVariations WHERE ProductId IN ({productIds})";
                
                using (var cmd = new SQLiteCommand(variationSql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var variation = new ProductVariation
                        {
                            Id = reader.GetInt32(0),
                            ProductId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Price = (decimal)reader.GetDouble(3)
                        };

                        if (products.ContainsKey(variation.ProductId))
                        {
                            products[variation.ProductId].Variations.Add(variation);
                        }
                    }
                }
            }

            return new List<Product>(products.Values);
        }

        public static List<Product> GetAllProducts()
        {
            var products = new Dictionary<int, Product>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                string productSql = "SELECT Id, Category, Name, Description, ImagePath FROM Products";
                using (var cmd = new SQLiteCommand(productSql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            Id = reader.GetInt32(0),
                            Category = reader.GetString(1),
                            Name = reader.GetString(2),
                            Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            ImagePath = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Variations = new List<ProductVariation>()
                        };
                        products[product.Id] = product;
                    }
                }

                string variationSql = "SELECT Id, ProductId, Name, Price FROM ProductVariations";
                using (var cmd = new SQLiteCommand(variationSql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var variation = new ProductVariation
                        {
                            Id = reader.GetInt32(0),
                            ProductId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Price = (decimal)reader.GetDouble(3)
                        };

                        if (products.ContainsKey(variation.ProductId))
                        {
                            products[variation.ProductId].Variations.Add(variation);
                        }
                    }
                }
            }

            return new List<Product>(products.Values);
        }
    }
}
