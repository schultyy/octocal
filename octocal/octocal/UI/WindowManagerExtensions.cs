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
            var container = IoC.Get<IWindsorContainer>();
            var viewModel = container.Resolve<T>();
            windowManager.ShowModal(viewModel);
        }

        public static void ShowModal<T>(this IWindowManager windowManager, T viewModel)
        {
            dynamic options = new ExpandoObject();

            options.WindowStyle = WindowStyle.None;
            options.ShowInTaskbar = false;
            options.AllowsTransparency = true;
            options.Background = new SolidColorBrush(Colors.Black);
            options.Foreground = new SolidColorBrush(Colors.White);
            options.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            options.Width = 400;
            options.Height = 300;
            options.SizeToContent = SizeToContent.Manual;

            windowManager.ShowDialog(viewModel, settings: options);
        }
    }
}
