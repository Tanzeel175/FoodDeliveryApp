using System.Windows;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Services;

namespace FoodDeliveryApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseHelper.Initialize();
            OrderService.Initialize();
        }
    }
}
