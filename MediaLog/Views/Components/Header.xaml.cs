using System.Windows;
using System.Windows.Controls;

namespace MediaLog.Views.Components
{
    public partial class Header : UserControl
    {
        public Header()
        {
            InitializeComponent();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.WindowState = window.WindowState == WindowState.Maximized
                                 ? WindowState.Normal
                                 : WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
