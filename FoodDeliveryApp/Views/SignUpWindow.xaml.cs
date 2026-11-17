using System.Windows;
using System.Windows.Input;
using FoodDeliveryApp.Services;

namespace FoodDeliveryApp.Views
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
            
            // Re-apply the template to fix the missing placeholder text for empty text boxes
            UsernameTextBox.Loaded += (s, e) => {
                var template = UsernameTextBox.Template;
                UsernameTextBox.Template = null;
                UsernameTextBox.Template = template;
            };
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordInput.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorText.Text = "Please fill in all fields.";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            bool success = AuthService.Register(username, password);
            if (success)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorText.Text = "Username already taken.";
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void LoginLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
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
