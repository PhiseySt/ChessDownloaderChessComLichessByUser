using ChessDownloader;
using ChessDownloader.ChessCom;
using ChessDownloader.Lichess;
using ChessDownloader.Pgn;

namespace CommandLineWork;

internal class ChessDownloaderConsole
{
    private const int ExitCodeSuccess = 0;

    public async Task<int> DownloadAllGames(string? source, string username, string output)
    {
        var chessDownloader = GetDownloaderBySourceName(source);

        var games = await chessDownloader.GetGamesByUsernameAsync(username, GetHandlerProcessMessage());

        await SaveAllGames(output, games);

        return ExitCodeSuccess;
    }

    private Task SaveAllGames(string output, ICollection<Game> games)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var finalPathOutput = Path.Combine(currentDirectory, output);

        Parallel.ForEach(games, game =>
        {
            var pgn = new PgnParser(game.Pgn).Parse();
            SavePgn(finalPathOutput, game, pgn);
        });


        Console.WriteLine($"{games.Count} games were saved at:");
        Console.WriteLine(finalPathOutput);

        return Task.CompletedTask;
    }

    private static void SavePgn(string output, Game game, PgnGame pgn)
    {

        var white = pgn.White;
        var black = pgn.Black;
        var date = pgn.UTCDate?.Replace(".", "");
        var time = pgn.UTCTime?.Replace(":", "");
        var sourceName = game.Source?.Name;

        var fileName = $"{date}.{time}.{sourceName}-{white}-{black}.pgn";
        Directory.CreateDirectory(output);
        var filePath = Path.Combine(output, fileName);
        File.WriteAllText(filePath, game.Pgn);
    }

    private static Progress<ChessDownloaderProgress> GetHandlerProcessMessage()
    {
        return new Progress<ChessDownloaderProgress>(progressValue =>
        {
            Console.WriteLine(progressValue.Undefined
                ? $"{progressValue.ProgressMessage}"
                : $"{progressValue.Position}/{progressValue.Total}% - {progressValue.ProgressMessage}");
        });
    }

    private static IChessDownloader GetDownloaderBySourceName(string? source)
    {
        IChessDownloader downloader;
        switch (source)
        {
            case "chesscom":
                downloader = new ChessComDownloader();
                break;
            case "lichess":
                downloader = new LichessDownloader();
                break;

            default:
                throw new ArgumentException($"downloader {source} not found");
        }

        return downloader;
    }
}