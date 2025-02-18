using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using MediaLog.Models;

namespace MediaLog.ViewModels
{
    public class MediaViewerViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public ObservableCollection<MediaItem> Items { get; set; }
        public ICommand ScrollLeftCommand { get; }
        public ICommand ScrollRightCommand { get; }
        public ICommand AddCommand { get; }

        public MediaViewerViewModel(string title, ObservableCollection<MediaItem> items, ICommand addCommand)
        {
            Title = title;
            Items = items;
            AddCommand = addCommand;
            ScrollLeftCommand = new RelayCommand<object>(ScrollLeft);
            ScrollRightCommand = new RelayCommand<object>(ScrollRight);
        }

        private void ScrollLeft(object parameter)
        {
            if (parameter is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 200);
            }
        }

        private void ScrollRight(object parameter)
        {
            if (parameter is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 200);
            }
        }
    }
}
