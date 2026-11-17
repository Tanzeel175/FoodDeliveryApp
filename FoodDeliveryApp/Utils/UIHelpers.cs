using System.Windows;
using System.Windows.Media;

namespace FoodDeliveryApp.Utils
{
    public static class UIHelpers
    {
        public static T GetParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent) return parent;
            return GetParent<T>(parentObject);
        }

        public static T FindVisualChild<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T element && element.Name == name)
                {
                    return element;
                }
                var result = FindVisualChild<T>(child, name);
                if (result != null) return result;
            }
            return null;
        }
    }
}
