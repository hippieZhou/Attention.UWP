﻿using Attention.Core.Dtos;
using Attention.Core.Services;
using AutoMapper;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Utility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Commands
{
    public class PixabayCommand : Command<Response<IEnumerable<WallpaperDto>>>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public bool IsEditorsChoice { get; set; } = true;
        public bool IsSafeSearch { get; set; } = true;
    }

    public class PixabayCommandHandler : ICommandHandler<PixabayCommand, Response<IEnumerable<WallpaperDto>>>
    {
        private readonly PixabaySharpClient _client;
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public PixabayCommandHandler(
            PixabaySharpClient client,
            IDataService dataService,
            IMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<IEnumerable<WallpaperDto>>> Handle(PixabayCommand request, CancellationToken cancellationToken)
        {
            var qb = new ImageQueryBuilder()
            {
                Page = request.Page,
                PerPage = request.PerPage,

                IsEditorsChoice = request.IsEditorsChoice,
                IsSafeSearch = request.IsSafeSearch,
                ResponseGroup = ResponseGroup.HighResolution
            };

            //var imageResult = await _client.QueryImagesAsync(qb);
            //var result = _mapper.Map<IEnumerable<WallpaperDto>>(imageResult?.Images);

            var result = await _dataService.GetWallpaperItems(qb.Page.Value, qb.PerPage.Value);

            return Response<IEnumerable<WallpaperDto>>.Success(result);
        }
    }
}