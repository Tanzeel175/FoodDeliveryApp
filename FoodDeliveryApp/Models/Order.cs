using System;

namespace FoodDeliveryApp.Models
{
    /// <summary>
    /// Represents a customer order in the food delivery system.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string Items { get; set; } // Serialized cart summary string
        public decimal TotalAmount { get; set; }
        public DateTime PlacedAt { get; set; }
        public string Status { get; set; } // "Pending", "Processing", "Completed"
    }
}
