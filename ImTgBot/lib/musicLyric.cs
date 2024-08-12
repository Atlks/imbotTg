using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace prjx.libx
{
  

    public class LyricsFetcher
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<string> GetLyricsAsync(string artist, string title)
        {
            try
            {
                string url = $"https://api.lyrics.ovh/v1/{Uri.EscapeDataString(artist)}/{Uri.EscapeDataString(title)}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var lyricsData = Newtonsoft.Json.JsonConvert.DeserializeObject<LyricsResponse>(responseBody);

                return lyricsData.Lyrics;
            }
            catch (HttpRequestException e)
            {
               Print($"Request error: {e.Message}");
                return "Lyrics not found.";
            }
        }

        private class LyricsResponse
        {
            public string Lyrics { get; set; }
        }
    }

    // 使用示例
    public class Program22
    {
        //public static void Main2(string[] args)
        //{
        //    LyricsFetcher fetcher = new LyricsFetcher();
        //    string lyrics = await fetcher.GetLyricsAsync("Coldplay", "Adventure of a Lifetime");
        //   print(lyrics);
        //}
    }

}
