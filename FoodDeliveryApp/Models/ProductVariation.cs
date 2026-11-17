namespace FoodDeliveryApp.Models
{
    /// <summary>
    /// Represents a size/variation option for a product, each with its own price.
    /// </summary>
    public class ProductVariation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public override string ToString() => $"{Name} - Rs {Price:N0}";
    }
}
