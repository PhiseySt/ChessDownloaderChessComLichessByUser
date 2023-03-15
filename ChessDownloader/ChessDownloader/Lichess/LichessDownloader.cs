using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ChessDownloader.Lichess.GamesByUserEndpoint;
using Newtonsoft.Json;

namespace ChessDownloader.Lichess
{
    public class LichessDownloader : IChessDownloader
    {
        private IProgress<ChessDownloaderProgress>? ChessDownloaderProgress;

        public async Task<IList<Game>> GetGamesByUsernameAsync(string username, IProgress<ChessDownloaderProgress>? progressMessage)
        {
            ChessDownloaderProgress = progressMessage;
            var jsonND = await GetJsonNdFromUsername(username);
            var games = GetAllGamesFromJsonNd(jsonND);

            return games;
        }

        private async Task<string> GetJsonNdFromUsername(string username)
        {
            var url = $"https://lichess.org/api/games/user/{username}?pgnInJson=true";
            var client = new HttpClient();
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/x-ndjson"));
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Username not found");
            }
            response.EnsureSuccessStatusCode();

            var memoryStream = new MemoryStream();
            await using (var downloadStream = await response.Content.ReadAsStreamAsync())
            {
                var buffer = new byte[8192];
                int bytesRead;

                while ((bytesRead = await downloadStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead); 
                    ChessDownloaderProgress?.Report(new ChessDownloaderProgress(memoryStream.Length, 0,
                        $"{memoryStream.Length} bytes were downloaded.", true));
                }
            }
            memoryStream.Position = 0;
            using var streamReader = new StreamReader(memoryStream);
            var jsonNd = await streamReader.ReadToEndAsync();
            return jsonNd;
        }

        

        private static List<Game> GetAllGamesFromJsonNd(string jsonNd)
        {
            string? line;
            var gameList = new List<Game>();
            using var sr = new StringReader(jsonNd);
            while ((line = sr.ReadLine()) != null)
            {
                var gameLichess = JsonConvert.DeserializeObject<GameLichessResponse>(line);
                if (gameLichess != null) gameList.Add(gameLichess);
            }

            return gameList;
        }
    }
}
