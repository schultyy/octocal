﻿using System.Dynamic;
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
            options.Background = new SolidColorBrush(Color.FromArgb(255, 242, 242, 242));

            windowManager.ShowDialog(viewModel, settings: options);
        }
    }
}
