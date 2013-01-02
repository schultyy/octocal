using System.Windows;

namespace octocal.UI.Services
{
    public interface IMessageBoxService
    {
        MessageBoxResult ShowYesNo(string message, string caption = null);
    }
}