using System.Windows;
using System.Windows.Controls;
using FoodDeliveryApp.Services;

namespace FoodDeliveryApp.Views
{
    public partial class AdminDashboardPage : Page
    {
        public AdminDashboardPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            var orders = OrderService.GetPendingOrders();
            int count = OrderService.GetPendingCount();

            PendingCountText.Text = count == 1
                ? "1 order in queue"
                : $"{count} orders in queue";

            if (orders == null || orders.Count == 0)
            {
                EmptyPanel.Visibility = Visibility.Visible;
                OrdersScrollViewer.Visibility = Visibility.Collapsed;
                ProcessNextButton.IsEnabled = false;
            }
            else
            {
                EmptyPanel.Visibility = Visibility.Collapsed;
                OrdersScrollViewer.Visibility = Visibility.Visible;
                ProcessNextButton.IsEnabled = true;

                OrdersItemsControl.ItemsSource = null;
                OrdersItemsControl.ItemsSource = orders;
            }
        }

        private void ProcessNext_Click(object sender, RoutedEventArgs e)
        {
            var order = OrderService.ProcessNextOrder();

            if (order != null)
            {
                CustomMessageBox.Show(
                    $"Order #{order.Id} has been processed!\n\n" +
                    $"Customer: {order.CustomerName}\n" +
                    $"Total: Rs {order.TotalAmount:N0}\n" +
                    $"Status: {order.Status}",
                    "Order Processed ✓",
                    MessageBoxImage.Information);
            }
            else
            {
                CustomMessageBox.Show(
                    "No pending orders to process.",
                    "Queue Empty",
                    MessageBoxImage.Information);
            }

            LoadOrders();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }
    }
}
