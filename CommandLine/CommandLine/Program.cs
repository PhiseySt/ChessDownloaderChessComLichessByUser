using CommandLine;

namespace CommandLineWork
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var parserResult = Parser.Default.ParseArguments<ChessDownloaderOptions>(args);

            await parserResult.WithParsedAsync(RunAndReturnExitCodeAsync);
            await parserResult.WithNotParsedAsync(HandleParseErrorAsync);
        }

        private static async Task<int> RunAndReturnExitCodeAsync(ChessDownloaderOptions options)
        {
            try
            {
                var chessDownloader = new ChessDownloaderConsole();
                return await chessDownloader.DownloadAllGames(options.Source, options.Username, options.Output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        private static Task HandleParseErrorAsync(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                if (error is VersionRequestedError)
                {
                }
            }
            return Task.CompletedTask;
        }


    }
}