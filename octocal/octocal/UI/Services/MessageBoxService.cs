using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace octocal.UI.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult ShowYesNo(string message, string caption = null)
        {
            caption = caption ?? string.Empty;
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
