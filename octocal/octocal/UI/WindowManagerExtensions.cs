using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
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

            var parentWindow = Application.Current.MainWindow;

            var width = ((ContentControl)parentWindow.Content).ActualWidth;
            var height = ((ContentControl)parentWindow.Content).ActualHeight;

            options.WindowStyle = WindowStyle.None;
            options.ShowInTaskbar = false;
            options.AllowsTransparency = true;
            options.Background = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            options.Foreground = new SolidColorBrush(Colors.White);
            options.WindowStartupLocation = WindowStartupLocation.Manual;
            options.Width = 600;
            options.Height = 400;
            options.Margin = new Thickness(10);
            options.Top = Application.Current.MainWindow.Top + 100;
            options.Left = Application.Current.MainWindow.Left + 100;
            options.SizeToContent = SizeToContent.Manual;



            windowManager.ShowDialog(viewModel, settings: options);
        }
    }
}
