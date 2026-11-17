using System.Windows;
using System.Windows.Controls;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services;

namespace FoodDeliveryApp.Views
{
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCart();
        }

        private void LoadCart()
        {
            var items = CartService.GetCartItems();

            if (items == null || items.Count == 0)
            {
                EmptyCartPanel.Visibility = Visibility.Visible;
                CartScrollViewer.Visibility = Visibility.Collapsed;
                BottomBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                EmptyCartPanel.Visibility = Visibility.Collapsed;
                CartScrollViewer.Visibility = Visibility.Visible;
                BottomBar.Visibility = Visibility.Visible;

                CartItemsControl.ItemsSource = null;
                CartItemsControl.ItemsSource = items;

                decimal total = CartService.GetTotal();
                TotalText.Text = $"Rs {total:N0}";
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var item = button.Tag as CartItem;
            if (item == null) return;

            CartService.RemoveFromCart(item);
            LoadCart();

            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.UpdateCartBadge();
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            var items = CartService.GetCartItems();
            if (items == null || items.Count == 0)
            {
                CustomMessageBox.Show("Your cart is empty!", "Empty Cart", MessageBoxImage.Information);
                return;
            }

            var user = AuthService.CurrentUser;
            var total = CartService.GetTotal();

            var order = OrderService.PlaceOrder(user, items, total);
            CartService.ClearCart();

            CustomMessageBox.Show(
                $"Order #{order.Id} placed successfully!\n\nTotal: Rs {total:N0}\n\nYour food is being prepared! 🍕",
                "Order Confirmed ✓",
                MessageBoxImage.Information);

            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.UpdateCartBadge();
            mainWindow?.NavigateToPage(new ProductDashboardPage());
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NavigateToPage(new CategoryDashboardPage());
        }
    }
}
