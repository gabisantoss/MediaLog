using System;
using System.Collections.Generic;
using System.Windows;
using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;
using MediaLog.Database;
using MediaLog.Views;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media;

namespace MediaLog
{

    public class MediaItem
    {
        public string? Title { get; set; }
        public string? ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
    };


    public partial class MainWindow : Window
    {

        private static string tmdbApiKey = "7f59255c69ee2bb6b8d4314c6d89f0fd"; // Substitua pela sua chave TMDb
        private static string lastFmApiKey = "b9b3b3b3b3b3b3b3b3b3b3b3b3b3b3"; // Substitua pela sua chave Last.fm

        public MainWindow()
        {
            InitializeComponent();
            LoadPopularMovies();
            LoadPopularSeries();
            LoadPopularAlbums();
        }

        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            Border card = sender as Border;
            if (card != null)
            {
                card.Effect = new DropShadowEffect
                {
                    BlurRadius = 15,
                    ShadowDepth = 4,
                    Opacity = 0.5,
                    Color = Colors.White
                };
            }
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            Border card = sender as Border;
            if (card != null)
            {
                card.Effect = new DropShadowEffect
                {
                    BlurRadius = 10,
                    ShadowDepth = 2,
                    Opacity = 0.3,
                    Color = Colors.Black
                };
            }
        }


        private void MoviesScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null && e.Delta != 0)
            {
                // Direciona o scroll para a janela principal
                MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }
    

        private void ScrollLeftMovies(object sender, RoutedEventArgs e)
        {
            MoviesScrollViewer.ScrollToHorizontalOffset(MoviesScrollViewer.HorizontalOffset - 200);
        }

        private void ScrollRightMovies(object sender, RoutedEventArgs e)
        {
            MoviesScrollViewer.ScrollToHorizontalOffset(MoviesScrollViewer.HorizontalOffset + 200);
        }

        private void ScrollLeftSeries(object sender, RoutedEventArgs e)
        {
            SeriesScrollViewer.ScrollToHorizontalOffset(SeriesScrollViewer.HorizontalOffset - 200);
        }

        private void ScrollRightSeries(object sender, RoutedEventArgs e)
        {
            SeriesScrollViewer.ScrollToHorizontalOffset(SeriesScrollViewer.HorizontalOffset + 200);
        }

        private void ScrollLeftAlbums(object sender, RoutedEventArgs e)
        {
            AlbumsScrollViewer.ScrollToHorizontalOffset(AlbumsScrollViewer.HorizontalOffset - 200);
        }

        private void ScrollRightAlbums(object sender, RoutedEventArgs e)
        {
            AlbumsScrollViewer.ScrollToHorizontalOffset(AlbumsScrollViewer.HorizontalOffset + 200);
        }



        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }


        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadPopularMovies()
        {
            var client = new RestClient("https://api.themoviedb.org/3/");
            var request = new RestRequest($"movie/popular?api_key={tmdbApiKey}&language=pt-BR", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful && response.Content != null)
            {
                var json = JObject.Parse(response.Content);
                var movies = json["results"];
                List<MediaItem> movieList = new List<MediaItem>();

                int count = 0;
                foreach (var movie in movies)
                {
                    if (count >= 10) break; // Mostrar apenas 5 itens

                    movieList.Add(new MediaItem
                    {
                        Title = movie["title"].ToString(),
                        ReleaseDate = $"📅 {movie["release_date"]}",
                        PosterPath = $"https://image.tmdb.org/t/p/w500{movie["poster_path"]}"
                    });

                    count++;
                }

                MoviesList.ItemsSource = movieList;
            }
        }

        private void LoadPopularSeries()
        {
            var client = new RestClient("https://api.themoviedb.org/3/");
            var request = new RestRequest($"tv/popular?api_key={tmdbApiKey}&language=pt-BR", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful && response.Content != null)
            {
                var json = JObject.Parse(response.Content);
                var series = json["results"];
                List<MediaItem> seriesList = new List<MediaItem>();

                int count = 0;
                
                foreach (var serie in series)
                {
                    if (count >= 10) break;
                    
                    seriesList.Add(new MediaItem
                    {
                        Title = serie["name"].ToString(),
                        ReleaseDate = $"📅 {serie["first_air_date"]}",
                        PosterPath = $"https://image.tmdb.org/t/p/w500{serie["poster_path"]}"
                    });

                    count++;
                }

                SeriesList.ItemsSource = seriesList;
            }
        }

        private void LoadPopularAlbums()
        {
            var client = new RestClient("http://ws.audioscrobbler.com/2.0/");
            var request = new RestRequest($"?method=chart.gettopalbums&api_key={lastFmApiKey}&format=json", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var json = JObject.Parse(response.Content);
                var albums = json["albums"]["album"];
                List<MediaItem> albumList = new List<MediaItem>();

                int count = 0;

                if (albums == null) return;
                foreach (var album in albums)
                {
                    if (count >= 5) break;

                    albumList.Add(new MediaItem
                    {
                        Title = album["name"].ToString(),
                        ReleaseDate = album["artist"]["name"].ToString(),
                        PosterPath = album["image"][2]["#text"].ToString() // Última imagem do JSON é a maior
                    });

                    count++;
                }

                AlbumsList.ItemsSource = albumList;
            }
        }

        private void AddMovieToLog(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string movieTitle)
            {
                SQLiteHelper.InsertMedia(movieTitle, "Filme", 0, ""); // Salva no banco de dados
                MessageBox.Show($"Filme '{movieTitle}' adicionado ao seu log!");
            }
        }

        private void OpenMoviesWindow(object sender, RoutedEventArgs e)
        {
            var moviesWindow = new MoviesWindow();
            moviesWindow.Show();
        }

        private void AddSeriesToLog(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string seriesTitle)
            {
                SQLiteHelper.InsertMedia(seriesTitle, "Série", 0, "");
                MessageBox.Show($"Série '{seriesTitle}' adicionada ao seu log!");
            }
        }

        private void OpenSeriesWindow(object sender, RoutedEventArgs e)
        {
            var seriesWindow = new SeriesWindow();
            seriesWindow.Show();
        }

        private void AddAlbumToLog(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string albumTitle)
            {
                SQLiteHelper.InsertMedia(albumTitle, "Álbum", 0, "");
                MessageBox.Show($"Álbum '{albumTitle}' adicionado ao seu log!");
            }
        }

        private void OpenAlbumsWindow(object sender, RoutedEventArgs e)
        {
            var albumsWindow = new AlbumsWindow();
            albumsWindow.Show();
        }
    }

}
