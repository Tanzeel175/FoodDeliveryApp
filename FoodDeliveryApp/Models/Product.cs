using System.Collections.Generic;

namespace FoodDeliveryApp.Models
{
    /// <summary>
    /// Represents a food product available for ordering.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<ProductVariation> Variations { get; set; } = new List<ProductVariation>();
    }
}
