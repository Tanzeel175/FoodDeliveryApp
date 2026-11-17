namespace FoodDeliveryApp.Models
{
    /// <summary>
    /// Represents a user in the food delivery system.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "customer" or "admin"
    }
}
