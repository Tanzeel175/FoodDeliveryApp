namespace FoodDeliveryApp.Models
{
    /// <summary>
    /// Represents a single item in the shopping cart, combining a product,
    /// a selected variation, and a quantity.
    /// </summary>
    public class CartItem
    {
        public Product Product { get; set; }
        public ProductVariation Variation { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Subtotal => Variation.Price * Quantity;

        public override bool Equals(object obj)
        {
            if (obj is CartItem other)
                return Product.Id == other.Product.Id && Variation.Id == other.Variation.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return (Product.Id * 397) ^ Variation.Id;
        }
    }
}
