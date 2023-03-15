namespace ChessDownloader.Pgn
{
    public class PgnParser
    {
        private const string EventKey = "Event";
        private const string SiteKey = "Site";
        private const string DateKey = "Date";
        private const string WhiteKey = "White";
        private const string BlackKey = "Black";
        private const string ResultKey = "Result";
        private const string TimezoneKey = "Timezone";
        private const string EcoKey = "ECO";
        private const string UtcdateKey = "UTCDate";
        private const string UtctimeKey = "UTCTime";
        private const string WhiteeloKey = "WhiteElo";
        private const string BlackeloKey = "BlackElo";
        private const string TimecontrolKey = "TimeControl";
        private const string TerminationKey = "Termination";
        private const string StarttimeKey = "StartTime";
        private const string EnddateKey = "EndDate";
        private const string EndtimeKey = "EndTime";
        private const string LinkKey = "Link";

        private readonly string[] _pgnLines;

        public PgnParser(string? pgnRaw)
        {
            _pgnLines = pgnRaw.Split("\n");
        }

        public PgnGame Parse()
        {
            var pgn = new PgnGame();
            foreach (var line in _pgnLines)
            {
                if (HasKey(EventKey, line))
                {
                    pgn.Event = Extract(EventKey, line);
                    continue;
                }

                if (HasKey(SiteKey, line))
                {
                    pgn.Site = Extract(SiteKey, line);
                    continue;
                }

                if (HasKey(DateKey, line))
                {
                    pgn.Date = Extract(DateKey, line);
                    continue;
                }

                if (HasKey(WhiteKey, line))
                {
                    pgn.White = Extract(WhiteKey, line);
                    continue;
                }

                if (HasKey(BlackKey, line))
                {
                    pgn.Black = Extract(BlackKey, line);
                    continue;
                }

                if (HasKey(ResultKey, line))
                {
                    pgn.Result = Extract(ResultKey, line);
                    continue;
                }

                if (HasKey(TimezoneKey, line))
                {
                    pgn.Timezone = Extract(TimezoneKey, line);
                    continue;
                }

                if (HasKey(EcoKey, line))
                {
                    pgn.ECO = Extract(EcoKey, line);
                    continue;
                }

                if (HasKey(UtcdateKey, line))
                {
                    pgn.UTCDate = Extract(UtcdateKey, line);
                    continue;
                }

                if (HasKey(UtctimeKey, line))
                {
                    pgn.UTCTime = Extract(UtctimeKey, line);
                    continue;
                }

                if (HasKey(WhiteeloKey, line))
                {
                    pgn.WhiteElo = Extract(WhiteeloKey, line);
                    continue;
                }

                if (HasKey(BlackeloKey, line))
                {
                    pgn.BlackElo = Extract(BlackeloKey, line);
                    continue;
                }

                if (HasKey(TimecontrolKey, line))
                {
                    pgn.TimeControl = Extract(TimecontrolKey, line);
                    continue;
                }

                if (HasKey(TerminationKey, line))
                {
                    pgn.Termination = Extract(TerminationKey, line);
                    continue;
                }

                if (HasKey(StarttimeKey, line))
                {
                    pgn.StartTime = Extract(StarttimeKey, line);
                    continue;
                }

                if (HasKey(EnddateKey, line))
                {
                    pgn.EndDate = Extract(EnddateKey, line);
                    continue;
                }

                if (HasKey(EndtimeKey, line))
                {
                    pgn.EndTime = Extract(EndtimeKey, line);
                    continue;
                }

                if (HasKey(LinkKey, line))
                {
                    pgn.Link = Extract(LinkKey, line);
                    continue;
                }
            }
            return pgn;
        }

        private static bool HasKey(string key, string line)
        {
            var keyTemplate = $"[{key} ";
            return line.StartsWith(keyTemplate);
        }

        private static string Extract(string key, string line)
        {
            var newLine = line.Replace("[" + key + " \"", "");
            return newLine.Replace("\"]", "");
        }
    }
}
