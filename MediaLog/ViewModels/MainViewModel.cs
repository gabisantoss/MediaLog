using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaLog.Models;
using MediaLog.Services;

namespace MediaLog.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly TmdbService _tmdbService;
        private readonly LastFmService _lastFmService;
        private readonly LogService _logService;

        public MediaViewerViewModel MoviesViewModel { get; }
        public MediaViewerViewModel SeriesViewModel { get; }
        public MediaViewerViewModel AlbumsViewModel { get; }

        public MainViewModel()
        {
            _tmdbService = new TmdbService();
            _lastFmService = new LastFmService();
            _logService = new LogService();

            MoviesViewModel = new MediaViewerViewModel("Filmes", new ObservableCollection<MediaItem>(), new RelayCommand<string>(AddMovieToLog));
            SeriesViewModel = new MediaViewerViewModel("Séries", new ObservableCollection<MediaItem>(), new RelayCommand<string>(AddSeriesToLog));
            AlbumsViewModel = new MediaViewerViewModel("Álbuns", new ObservableCollection<MediaItem>(), new RelayCommand<string>(AddAlbumToLog));

            LoadData();
        }

        private async void LoadData()
        {
            MoviesViewModel.Items.Clear();
            var movies = await _tmdbService.GetPopularMovies();
            foreach (var movie in movies)
                MoviesViewModel.Items.Add(movie);

            SeriesViewModel.Items.Clear();
            var series = await _tmdbService.GetPopularSeries();
            foreach (var serie in series)
                SeriesViewModel.Items.Add(serie);

            AlbumsViewModel.Items.Clear();
            var albums = await _lastFmService.GetPopularAlbums();
            foreach (var album in albums)
                AlbumsViewModel.Items.Add(album);
        }

        private void AddMovieToLog(string title) => _logService.AddToLog(title, "Filme");

        private void AddSeriesToLog(string title) => _logService.AddToLog(title, "Série");

        private void AddAlbumToLog(string title) => _logService.AddToLog(title, "Álbum");
    }
}
