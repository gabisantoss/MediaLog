using RestSharp;
using Newtonsoft.Json.Linq;
using System;

namespace MediaLog.Services
{
    public static class MovieService
    {
        private static string apiKey = "7f59255c69ee2bb6b8d4314c6d89f0fd";  // Replace with your TMDb API key

        public static string GetMovieDetails(string movieTitle)
        {
            var client = new RestClient("https://api.themoviedb.org/3/");
            var request = new RestRequest($"search/movie?query={movieTitle}&api_key={apiKey}");

            var response = client.Execute(request);

            if (response.IsSuccessful && response.Content != null)
            {
                var json = JObject.Parse(response.Content);
                var firstMovie = json["results"]?.First;

                if (firstMovie != null)
                {
                    return $"{firstMovie["title"]} ({firstMovie["release_date"]}) - {firstMovie["overview"]}";
                }
                else
                {
                    return "Movie not found.";
                }
            }
            return "Error fetching data.";
        }
    }
}
