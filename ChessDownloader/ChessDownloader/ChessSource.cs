namespace ChessDownloader
{
    public class ChessSource
    {
        private ChessSource(string name) { Name = name; }
        public string Name { get; }
        public static ChessSource ChessCom => new ChessSource("ChessCom");
        public static ChessSource Lichess => new ChessSource("Lichess");
    }
}
