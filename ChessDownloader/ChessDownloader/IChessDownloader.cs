using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChessDownloader
{
    public interface IChessDownloader
    {
        Task<IList<Game>> GetGamesByUsernameAsync(string username,
            IProgress<ChessDownloaderProgress>? progressMessage = null);
    }
}
