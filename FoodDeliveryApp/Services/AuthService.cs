using System.Data.SQLite;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services
{
    /// <summary>
    /// Provides authentication services including login, logout, and
    /// tracking the currently logged-in user.
    /// </summary>
    public static class AuthService
    {
        /// <summary>
        /// Gets the currently logged-in user, or null if no user is logged in.
        /// </summary>
        public static User CurrentUser { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current user has the "admin" role.
        /// </summary>
        public static bool IsAdmin => CurrentUser?.Role == "admin";

        /// <summary>
        /// Attempts to log in a user with the given credentials.
        /// </summary>
        /// <param name="username">The username to authenticate.</param>
        /// <param name="password">The password to authenticate.</param>
        /// <returns>The <see cref="User"/> if credentials are valid; otherwise, <c>null</c>.</returns>
        public static User Login(string username, string password)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT Id, Username, Password, Role FROM Users WHERE Username = @username AND Password = @password";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CurrentUser = new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                Role = reader.GetString(3)
                            };
                            return CurrentUser;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Registers a new customer user.
        /// </summary>
        public static bool Register(string username, string password)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                // Check if username already exists
                string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                using (var checkCmd = new SQLiteCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);
                    long count = (long)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        return false; // Username taken
                    }
                }

                // Insert new user
                string sql = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, 'customer')";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                }
                
                // Automatically log them in after registration
                return Login(username, password) != null;
            }
        }

        /// <summary>
        /// Logs out the current user by clearing the <see cref="CurrentUser"/> reference.
        /// </summary>
        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
