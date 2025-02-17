using MediaLog.Database;
using System.Diagnostics;
using System.Windows;

namespace MediaLog.Views
{
    public partial class LogsWindow : Window
    {
        public LogsWindow()
        {
            InitializeComponent();
            LoadLogs();
        }

        private void LoadLogs()
        {
            LogList.Items.Clear();
            var logs = SQLiteHelper.GetAllMedia();

            foreach (var log in logs)
            {
                LogList.Items.Add(log);
            }
        }

        private void CloseLogs(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}