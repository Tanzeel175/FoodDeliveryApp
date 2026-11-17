using System;
using System.Data.SQLite;
using System.IO;

namespace FoodDeliveryApp.Data
{
    public static class DatabaseHelper
    {
        private static string _dbPath;
        private static string _connectionString;

        static DatabaseHelper()
        {
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fooddelivery_v2.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";
            Initialize();
        }

        public static void Initialize()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }

            using (var connection = GetConnection())
            {
                CreateTables(connection);
                SeedData(connection);
            }
        }

        public static SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private static void CreateTables(SQLiteConnection connection)
        {
            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Role TEXT NOT NULL DEFAULT 'customer'
                );";

            string createProductsTable = @"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Category TEXT NOT NULL,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    ImagePath TEXT
                );";

            string createProductVariationsTable = @"
                CREATE TABLE IF NOT EXISTS ProductVariations (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductId INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    FOREIGN KEY (ProductId) REFERENCES Products(Id)
                );";

            string createOrdersTable = @"
                CREATE TABLE IF NOT EXISTS Orders (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    CustomerName TEXT,
                    Items TEXT,
                    TotalAmount REAL NOT NULL,
                    PlacedAt TEXT NOT NULL,
                    Status TEXT NOT NULL DEFAULT 'Pending',
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );";

            using (var cmd = new SQLiteCommand(createUsersTable, connection)) cmd.ExecuteNonQuery();
            using (var cmd = new SQLiteCommand(createProductsTable, connection)) cmd.ExecuteNonQuery();
            using (var cmd = new SQLiteCommand(createProductVariationsTable, connection)) cmd.ExecuteNonQuery();
            using (var cmd = new SQLiteCommand(createOrdersTable, connection)) cmd.ExecuteNonQuery();
        }

        private static void SeedData(SQLiteConnection connection)
        {
            using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Products", connection))
            {
                long productCount = (long)cmd.ExecuteScalar();
                if (productCount > 0)
                    return; // Data already seeded
            }

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Seed Users
                    SeedUser(connection, "customer1", "password1", "customer");
                    SeedUser(connection, "admin", "admin123", "admin");

                    var rand = new Random();

                    // --- BURGERS (9 Types) ---
                    var burgerData = new[] {
                        new { Name = "Classic Burger", Image = "Assets/Images/classic_burger.png" },
                        new { Name = "Zinger Burger", Image = "Assets/Images/zinger_burger.png" },
                        new { Name = "Patty Burger", Image = "Assets/Images/patty_burger.png" },
                        new { Name = "Cheese Burger", Image = "Assets/Images/cheese_burger.png" },
                        new { Name = "BBQ Bacon Burger", Image = "Assets/Images/bbq_bacon_burger.png" },
                        new { Name = "Mushroom Swiss", Image = "Assets/Images/mushroom_swiss.png" },
                        new { Name = "Veggie Burger", Image = "Assets/Images/veggie_burger.png" },
                        new { Name = "Spicy Chicken Burger", Image = "Assets/Images/spicy_chicken_burger.png" },
                        new { Name = "Fish Burger", Image = "Assets/Images/fish_burger.png" }
                    };
                    foreach (var b in burgerData) {
                        long id = SeedProduct(connection, "Burger", b.Name, $"Delicious {b.Name} with fresh ingredients.", b.Image);
                        int basePrice = rand.Next(14, 20) * 50;
                        SeedVariation(connection, id, "Single", basePrice);
                        SeedVariation(connection, id, "Double", basePrice + 400);
                        SeedVariation(connection, id, "Meal Combo", basePrice + 900);
                    }

                    // --- PIZZA (12 Types) ---
                    var pizzaData = new[] {
                        new { Name = "Margherita", Image = "Assets/Images/margherita_pizza.png" },
                        new { Name = "Pepperoni", Image = "Assets/Images/pepperoni_pizza.png" },
                        new { Name = "BBQ Chicken", Image = "Assets/Images/bbq_chicken_pizza.png" },
                        new { Name = "Hawaiian", Image = "Assets/Images/hawaiian_pizza.png" },
                        new { Name = "Veggie Supreme", Image = "Assets/Images/veggie_supreme_pizza.png" },
                        new { Name = "Meat Lovers", Image = "Assets/Images/meat_lovers_pizza.png" },
                        new { Name = "Four Cheese", Image = "Assets/Images/four_cheese_pizza.png" },
                        new { Name = "Buffalo Chicken", Image = "Assets/Images/buffalo_chicken_pizza.png" },
                        new { Name = "Pesto Chicken", Image = "Assets/Images/pesto_chicken_pizza.png" },
                        new { Name = "Spinach Feta", Image = "Assets/Images/spinach_feta_pizza.png" },
                        new { Name = "Mushroom Truffle", Image = "Assets/Images/mushroom_truffle_pizza.png" },
                        new { Name = "Garlic Parmesan", Image = "Assets/Images/garlic_parmesan_pizza.png" }
                    };
                    foreach (var p in pizzaData) {
                        long id = SeedProduct(connection, "Pizza", $"{p.Name} Pizza", $"Authentic {p.Name} pizza baked to perfection.", p.Image);
                        int basePrice = rand.Next(20, 30) * 50;
                        SeedVariation(connection, id, "Small", basePrice);
                        SeedVariation(connection, id, "Medium", basePrice + 600);
                        SeedVariation(connection, id, "Large", basePrice + 1200);
                    }

                    // --- CHICKEN WINGS (9 Types) ---
                    var wingsData = new[] {
                        new { Name = "Classic Buffalo", Image = "Assets/Images/classic_buffalo_wings.png" },
                        new { Name = "BBQ Honey", Image = "Assets/Images/bbq_honey_wings.png" },
                        new { Name = "Garlic Parmesan", Image = "Assets/Images/garlic_parmesan_wings.png" },
                        new { Name = "Lemon Pepper", Image = "Assets/Images/lemon_pepper_wings.png" },
                        new { Name = "Spicy Teriyaki", Image = "Assets/Images/spicy_teriyaki_wings.png" },
                        new { Name = "Mango Habanero", Image = "Assets/Images/mango_habanero_wings.png" },
                        new { Name = "Sweet Chili", Image = "Assets/Images/sweet_chili_wings.png" },
                        new { Name = "Smoked Chipotle", Image = "Assets/Images/smoked_chipotle_wings.png" },
                        new { Name = "Naked Wings", Image = "Assets/Images/naked_wings.png" }
                    };
                    foreach (var w in wingsData) {
                        long id = SeedProduct(connection, "Wings", w.Name, $"Crispy {w.Name} served hot.", w.Image);
                        int basePrice = rand.Next(16, 24) * 50;
                        SeedVariation(connection, id, "6 Pieces", basePrice);
                        SeedVariation(connection, id, "12 Pieces", basePrice + 700);
                        SeedVariation(connection, id, "24 Pieces", basePrice + 1850);
                    }

                    // --- SALADS (9 Types) ---
                    var saladData = new[] {
                        new { Name = "Caesar Salad", Image = "Assets/Images/caesar_salad.png" },
                        new { Name = "Greek Salad", Image = "Assets/Images/greek_salad.png" },
                        new { Name = "Garden House", Image = "Assets/Images/garden_house_salad.png" },
                        new { Name = "Cobb Salad", Image = "Assets/Images/cobb_salad.png" },
                        new { Name = "Chef Salad", Image = "Assets/Images/chef_salad.png" },
                        new { Name = "Caprese Salad", Image = "Assets/Images/caprese_salad.png" },
                        new { Name = "Spinach Apple", Image = "Assets/Images/spinach_apple_salad.png" },
                        new { Name = "Southwest Chicken", Image = "Assets/Images/southwest_chicken_salad.png" },
                        new { Name = "Asian Crunch", Image = "Assets/Images/asian_crunch_salad.png" }
                    };
                    foreach (var s in saladData) {
                        long id = SeedProduct(connection, "Salad", s.Name, $"Fresh and healthy {s.Name}.", s.Image);
                        int basePrice = rand.Next(14, 22) * 50;
                        SeedVariation(connection, id, "Regular", basePrice);
                        SeedVariation(connection, id, "Large", basePrice + 450);
                        SeedVariation(connection, id, "Add Protein", basePrice + 800);
                    }

                    // --- PASTA (9 Types) ---
                    var pastaData = new[] {
                        new { Name = "Pasta Alfredo", Image = "Assets/Images/pasta_alfredo.png" },
                        new { Name = "Spaghetti Bolognese", Image = "Assets/Images/spaghetti_bolognese.png" },
                        new { Name = "Penne Arrabbiata", Image = "Assets/Images/penne_arrabbiata.png" },
                        new { Name = "Mac & Cheese", Image = "Assets/Images/mac_and_cheese.png" },
                        new { Name = "Lasagna", Image = "Assets/Images/lasagna.png" },
                        new { Name = "Pesto Pasta", Image = "Assets/Images/pesto_pasta.png" },
                        new { Name = "Carbonara", Image = "Assets/Images/carbonara.png" },
                        new { Name = "Seafood Linguine", Image = "Assets/Images/seafood_linguine.png" },
                        new { Name = "Baked Ziti", Image = "Assets/Images/baked_ziti.png" }
                    };
                    foreach (var p in pastaData) {
                        long id = SeedProduct(connection, "Pasta", p.Name, $"Classic Italian {p.Name}.", p.Image);
                        int basePrice = rand.Next(18, 26) * 50;
                        SeedVariation(connection, id, "Half Portion", basePrice);
                        SeedVariation(connection, id, "Full Portion", basePrice + 850);
                        SeedVariation(connection, id, "Family Size", basePrice + 2100);
                    }

                    // --- MILKSHAKES (9 Types) ---
                    var shakeData = new[] {
                        new { Name = "Chocolate Milkshake", Image = "Assets/Images/chocolate_milkshake.png" },
                        new { Name = "Vanilla Bean", Image = "Assets/Images/vanilla_bean.png" },
                        new { Name = "Strawberry Delight", Image = "Assets/Images/strawberry_delight.png" },
                        new { Name = "Oreo Cookie", Image = "Assets/Images/oreo_cookie.png" },
                        new { Name = "Mint Chocolate", Image = "Assets/Images/mint_chocolate.png" },
                        new { Name = "Peanut Butter", Image = "Assets/Images/peanut_butter.png" },
                        new { Name = "Salted Caramel", Image = "Assets/Images/salted_caramel.png" },
                        new { Name = "Banana Split", Image = "Assets/Images/banana_split.png" },
                        new { Name = "Coffee Mocha", Image = "Assets/Images/coffee_mocha.png" }
                    };
                    foreach (var s in shakeData) {
                        long id = SeedProduct(connection, "Milkshake", s.Name, $"Thick and creamy {s.Name}.", s.Image);
                        int basePrice = rand.Next(9, 15) * 50;
                        SeedVariation(connection, id, "Small", basePrice);
                        SeedVariation(connection, id, "Medium", basePrice + 250);
                        SeedVariation(connection, id, "Large", basePrice + 450);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static void SeedUser(SQLiteConnection connection, string username, string password, string role)
        {
            // Simple check to ensure we don't insert duplicate default users if recreating only some data
            string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = @username";
            using (var cmd = new SQLiteCommand(checkSql, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                if ((long)cmd.ExecuteScalar() > 0) return;
            }

            string sql = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.ExecuteNonQuery();
            }
        }

        private static long SeedProduct(SQLiteConnection connection, string category, string name, string description, string imagePath)
        {
            string sql = "INSERT INTO Products (Category, Name, Description, ImagePath) VALUES (@category, @name, @description, @imagePath)";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@imagePath", imagePath);
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new SQLiteCommand("SELECT last_insert_rowid()", connection))
            {
                return (long)cmd.ExecuteScalar();
            }
        }

        private static void SeedVariation(SQLiteConnection connection, long productId, string name, double price)
        {
            string sql = "INSERT INTO ProductVariations (ProductId, Name, Price) VALUES (@productId, @name, @price)";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
