using System.Windows;

namespace FoodDeliveryApp.Views
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message, string title, MessageBoxImage icon)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;

            switch (icon)
            {
                case MessageBoxImage.Information:
                    IconText.Text = "ℹ️";
                    break;
                case MessageBoxImage.Warning:
                    IconText.Text = "⚠️";
                    break;
                case MessageBoxImage.Error:
                    IconText.Text = "❌";
                    break;
                case MessageBoxImage.Question:
                    IconText.Text = "❓";
                    break;
                default:
                    IconText.Text = "💬";
                    break;
            }
        }

        public static void Show(string message, string title = "Notification", MessageBoxImage icon = MessageBoxImage.Information)
        {
            var msgBox = new CustomMessageBox(message, title, icon);
            msgBox.ShowDialog();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
