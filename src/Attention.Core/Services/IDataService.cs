using Attention.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Attention.Core.Services
{
    public interface IDataService
    {
        Task<IEnumerable<WallpaperDto>> GetWallpaperItems(int page, int perPage = 10);
        Task<IEnumerable<DownloadDto>> GetDownloadItems(int page, int perPage = 10);
    }
}
