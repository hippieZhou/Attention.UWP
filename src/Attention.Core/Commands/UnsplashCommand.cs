using Attention.Core.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unsplasharp;

namespace Attention.Core.Commands
{
    public class UnsplashCommand : Command<Response<IEnumerable<WallpaperDto>>>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
    }

    public class UnsplashCommandHandler : ICommandHandler<UnsplashCommand, Response<IEnumerable<WallpaperDto>>>
    {
        private readonly UnsplasharpClient _client;
        private readonly IMapper _mapper;

        public UnsplashCommandHandler(UnsplasharpClient client, IMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<IEnumerable<WallpaperDto>>> Handle(UnsplashCommand request, CancellationToken cancellationToken)
        {
            var listPhotos = await _client.ListPhotos(page: request.Page, perPage: request.PerPage, orderBy: UnsplasharpClient.OrderBy.Popular);
            var result = _mapper.Map<IEnumerable<WallpaperDto>>(listPhotos);
            return Response<IEnumerable<WallpaperDto>>.Success(result);
        }
    }
}
