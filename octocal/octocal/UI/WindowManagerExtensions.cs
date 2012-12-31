using System.Dynamic;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Castle.Windsor;

namespace octocal.UI
{
    public static class WindowManagerExtensions
    {
        public static void ShowModal<T>(this IWindowManager windowManager)
        {
            var window = new Window();
            
            dynamic options = new ExpandoObject();

            options.WindowStyle = WindowStyle.None;
            options.ShowInTaskbar = false;
            options.AllowsTransparency = true;
            options.Background = new SolidColorBrush(Colors.Transparent);
            var container = IoC.Get<IWindsorContainer>();
            windowManager.ShowDialog(container.Resolve<T>(), settings: options);
        }
    }
}
