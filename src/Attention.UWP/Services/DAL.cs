using Attention.UWP.Models.Repositories;
using System;

namespace Attention.UWP.Services
{
    public class DAL
    {
        private readonly string _dbPath = string.Empty;
        public DownloadRepository DownloadRepo => new DownloadRepository(_dbPath);
        public DAL(string dbPath)
        {
            _dbPath = dbPath ?? throw new ArgumentNullException(nameof(dbPath));
        }
    }
}
