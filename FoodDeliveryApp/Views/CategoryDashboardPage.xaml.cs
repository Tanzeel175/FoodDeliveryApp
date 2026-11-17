using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FoodDeliveryApp.Services;

namespace FoodDeliveryApp.Views
{
    public partial class CategoryDashboardPage : Page
    {
        public CategoryDashboardPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categoryNames = ProductService.GetCategories();
            
            var categories = categoryNames.Select(name => new CategoryItem
            {
                Name = name,
                ImagePath = GetImagePathForCategory(name)
            }).ToList();

            CategoriesItemsControl.ItemsSource = categories;
        }

        private string GetImagePathForCategory(string categoryName)
        {
            switch (categoryName.ToLower())
            {
                case "burger": return "/Assets/Images/classic_burger.png";
                case "pizza": return "/Assets/Images/pepperoni_pizza.png";
                case "wings": return "/Assets/Images/classic_buffalo_wings.png";
                case "salad": return "/Assets/Images/caesar_salad.png";
                case "pasta": return "/Assets/Images/pasta_alfredo.png";
                case "milkshake": return "/Assets/Images/chocolate_milkshake.png";
                default: return "";
            }
        }

        private void CategoryTile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is CategoryItem categoryItem)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.NavigateToPage(new ProductDashboardPage(categoryItem.Name));
                }
            }
        }
    }

    public class CategoryItem
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
