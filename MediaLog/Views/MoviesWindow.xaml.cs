using System;
using System.Collections.Generic;
using System.Windows;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace MediaLog
{
    public partial class MoviesWindow : Window
    {
        private static string apiKey = "7f59255c69ee2bb6b8d4314c6d89f0fd"; // Substitua pela sua chave TMDb

        public MoviesWindow()
        {
            InitializeComponent();
            LoadAllMovies();
        }

        private void LoadAllMovies()
        {
            var client = new RestClient("https://api.themoviedb.org/3/");
            var request = new RestRequest($"movie/popular?api_key={apiKey}&language=pt-BR", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var json = JObject.Parse(response.Content);
                var movies = json["results"];
                List<string> movieTitles = new List<string>();

                foreach (var movie in movies)
                {
                    movieTitles.Add($"{movie["title"]} ({movie["release_date"]})");
                }

                MoviesFullList.ItemsSource = movieTitles;
            }
            else
            {
                MessageBox.Show("Erro ao buscar filmes populares.");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
