using System.Threading;
using System.Threading.Tasks;
using System;
using Attention.Core.Services;

namespace Attention.Core.Commands
{
    public class DownloadCommand : Command<Response>
    {
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string DownloadUri { get; set; }
    }

    public class DownloadCommandHandler : ICommandHandler<DownloadCommand, Response>
    {
        private readonly IDownloadService _downloadService;

        public DownloadCommandHandler(IDownloadService downloadService)
        {
            _downloadService = downloadService ?? throw new ArgumentNullException(nameof(downloadService));
        }

        public async Task<Response> Handle(DownloadCommand request, CancellationToken cancellationToken)
        {
            await _downloadService.Download(request.FolderName, request.FileName, request.DownloadUri);
            return Response.Success();
        }
    }
}
