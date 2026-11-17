using System.Windows;
using System.Windows.Controls;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Utils;

namespace FoodDeliveryApp.Views
{
    public partial class ProductDashboardPage : Page
    {
        private string _category;

        public ProductDashboardPage(string category = null)
        {
            InitializeComponent();
            _category = category;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_category))
            {
                CategoryTitleText.Text = $"🍽️ {_category} Menu";
            }
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = string.IsNullOrEmpty(_category) 
                ? ProductService.GetAllProducts() 
                : ProductService.GetProductsByCategory(_category);
                
            ProductsItemsControl.ItemsSource = products;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPage(new CategoryDashboardPage());
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var product = button.Tag as Product;
            if (product == null) return;

            var card = UIHelpers.GetParent<Border>(button);
            if (card == null) return;

            var variationCombo = UIHelpers.FindVisualChild<ComboBox>(card, "VariationCombo");
            var quantityCombo = UIHelpers.FindVisualChild<ComboBox>(card, "QuantityCombo");

            if (variationCombo == null || variationCombo.SelectedItem == null)
            {
                CustomMessageBox.Show("Please select a variation.", "Selection Required", MessageBoxImage.Warning);
                return;
            }

            int quantity = 1;
            if (quantityCombo != null && quantityCombo.SelectedItem is ComboBoxItem selectedItem)
            {
                int.TryParse(selectedItem.Content.ToString(), out quantity);
            }

            var variation = variationCombo.SelectedItem as ProductVariation;
            if (variation == null) return;

            var cartItem = new CartItem
            {
                Product = product,
                Variation = variation,
                Quantity = quantity
            };

            CartService.AddToCart(cartItem);

            // Update cart badge
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.UpdateCartBadge();

            // Brief visual confirmation
            button.Content = "✓ Added!";
            button.IsEnabled = false;

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(1.2);
            timer.Tick += (s, args) =>
            {
                button.Content = "🛒  Add to Cart";
                button.IsEnabled = true;
                timer.Stop();
            };
            timer.Start();
        }

        private void ViewCart_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NavigateToPage(new CartPage());
        }
    }
}
