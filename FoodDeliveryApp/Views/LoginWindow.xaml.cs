using System.Windows;
using System.Windows.Input;
using FoodDeliveryApp.Services;

namespace FoodDeliveryApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordInput.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorText.Text = "Please enter both username and password";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            var user = AuthService.Login(username, password);

            if (user == null)
            {
                ErrorText.Text = "Invalid username or password";
                ErrorText.Visibility = Visibility.Visible;
                PasswordInput.Clear();
                return;
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void SignUpLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var signUpWindow = new SignUpWindow();
            signUpWindow.Show();
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
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
    }
}
