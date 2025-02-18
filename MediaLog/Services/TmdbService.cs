using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLog.Models;

namespace MediaLog.Services
{
    public class TmdbService
    {
        private static readonly string tmdbApiKey = "7f59255c69ee2bb6b8d4314c6d89f0fd"; // Substitua pela sua chave TMDb
        private static readonly string baseUrl = "https://api.themoviedb.org/3/";

        private async Task<List<MediaItem>> FetchMediaItems(string endpoint)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest($"{endpoint}?api_key={tmdbApiKey}&language=en-US", Method.Get);
            var response = await client.ExecuteAsync(request);

            var mediaList = new List<MediaItem>();

            if (response.IsSuccessful && response.Content != null)
            {
                var json = JObject.Parse(response.Content);
                var results = json["results"];

                int count = 0;
                foreach (var item in results)
                {
                    if (count >= 15) break;

                    mediaList.Add(new MediaItem
                    {
                        Title = item["title"]?.ToString() ?? item["name"]?.ToString(),
                        ReleaseDate = $"📅 {item["release_date"] ?? item["first_air_date"]}",
                        PosterPath = $"https://image.tmdb.org/t/p/w500{item["poster_path"]}"
                    });

                    count++;
                }
            }

            return mediaList;
        }

        public async Task<List<MediaItem>> GetPopularMovies() => await FetchMediaItems("trending/movie/week");

        public async Task<List<MediaItem>> GetPopularSeries() => await FetchMediaItems("trending/tv/week");
    }
}
