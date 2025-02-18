using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLog.Models;

namespace MediaLog.Services
{
    public class LastFmService
    {
        private static readonly string lastFmApiKey = "b9b3b3b3b3b3b3b3b3b3b3b3b3b3b3"; // Substitua pela sua chave Last.fm
        private static readonly string baseUrl = "http://ws.audioscrobbler.com/2.0/";

        public async Task<List<MediaItem>> GetPopularAlbums()
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest($"?method=chart.gettopalbums&api_key={lastFmApiKey}&format=json", Method.Get);
            var response = await client.ExecuteAsync(request);

            var albumList = new List<MediaItem>();

            if (response.IsSuccessful && response.Content != null)
            {
                var json = JObject.Parse(response.Content);
                var albums = json["albums"]["album"];

                int count = 0;
                foreach (var album in albums)
                {
                    if (count >= 10) break;

                    albumList.Add(new MediaItem
                    {
                        Title = album["name"].ToString(),
                        ReleaseDate = album["artist"]["name"].ToString(),
                        PosterPath = album["image"][2]["#text"].ToString()
                    });

                    count++;
                }
            }

            return albumList;
        }
    }
}
