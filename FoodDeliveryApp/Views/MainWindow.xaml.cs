using System.Windows;
using System.Windows.Controls;
using FoodDeliveryApp.Services;

namespace FoodDeliveryApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var user = AuthService.CurrentUser;
            if (user == null)
            {
                new LoginWindow().Show();
                this.Close();
                return;
            }

            WelcomeText.Text = $"Welcome, {user.Username} 👋";

            if (user.Role == "admin")
            {
                CartButton.Visibility = Visibility.Collapsed;
                MainFrame.Navigate(new AdminDashboardPage());
            }
            else
            {
                UpdateCartBadge();
                MainFrame.Navigate(new CategoryDashboardPage());
            }
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CartPage());
        }

        private void Navigation_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            UpdateCartBadge();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            AuthService.Logout();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        public void UpdateCartBadge()
        {
            var badge = FindCartBadgeText();
            if (badge != null)
            {
                int count = CartService.GetItemCount();
                badge.Text = count.ToString();
            }
        }

        private TextBlock FindCartBadgeText()
        {
            // The badge TextBlock is inside the CartButton's template
            // We access it by walking the visual tree from the template
            if (CartButton.Template != null)
            {
                var badgeCount = CartButton.Template.FindName("BadgeCount", CartButton) as TextBlock;
                return badgeCount;
            }
            return null;
        }

        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
