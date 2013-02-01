using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using octocal.UI.Calendar.ViewModels;

namespace octocal.UI.Controls
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("DateTime", typeof(DateTimeViewModel), typeof(DateTimePicker), new PropertyMetadata(default(DateTimeViewModel)));

        public DateTimeViewModel DateTime
        {
            get { return (DateTimeViewModel)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }


        public DateTimePicker()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
