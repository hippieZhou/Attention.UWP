using Attention.Core.Dtos;
using Attention.Core.Extensions;
using Bogus;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Services
{
    public class DesignDataService : IDataService
    {
        public static readonly IEnumerable<Brush> Brushs = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null)).Select(x => new AcrylicBrush
        {
            BackgroundSource = AcrylicBackgroundSource.Backdrop,
            TintColor = x,
            FallbackColor = x,
            TintOpacity = 1.0,
            TintLuminosityOpacity = 1.0,
        });

        private readonly ICollection<string> _localImages = new List<string>
        {
            "ms-appx:///Assets/Images/00.jpg",
            "ms-appx:///Assets/Images/01.jpg",
            "ms-appx:///Assets/Images/02.jpg",
            "ms-appx:///Assets/Images/03.jpg",
            "ms-appx:///Assets/Images/04.jpg",
            "ms-appx:///Assets/Images/05.jpg",
            "ms-appx:///Assets/Images/06.jpg",
            "ms-appx:///Assets/Images/07.jpg",
            "ms-appx:///Assets/Images/08.jpg",
            "ms-appx:///Assets/Images/09.jpg"
        };

        public async Task<IEnumerable<WallpaperDto>> GetWallpaperItems(int page, int perPage = 10)
        {
            ICollection<Brush> brushs = Brushs.ToList();

            var generator = new Faker<WallpaperDto>()
              .RuleFor(i => i.Background, f => f.Random.CollectionItem(brushs))
              .RuleFor(i => i.Location, f => f.Person.Address.State)
              .RuleFor(i => i.Thumbnail, f => f.Random.CollectionItem(_localImages))
              .RuleFor(i => i.Title, f => f.Lorem.Word())
              .RuleFor(i => i.Description, f => string.Join(" ", f.Lorem.Words(20)))
              .RuleFor(i => i.ImageUri, f => f.Random.CollectionItem(_localImages))
              .RuleFor(i => i.ImageDownloadUri, f => f.Random.CollectionItem(_localImages))
              .RuleFor(i => i.Author, f => f.Person.FullName)
              .RuleFor(i => i.AuthorUri, f => f.Person.Avatar)
              .RuleFor(i => i.AuthorAvatar, f => f.Person.Avatar);

            await Task.Delay(1000);

            return generator.GenerateForever().Skip(page * perPage).Take(perPage);
        }

        public async Task<IEnumerable<DownloadDto>> GetDownloadItems(int page, int perPage = 10)
        {
            var generator = new Faker<DownloadDto>()
                .RuleFor(i => i.Background, f => Brushs.Random())
                .RuleFor(i => i.Title, f => f.Lorem.Slug())
                .RuleFor(i => i.Thumbnail, f => f.Random.CollectionItem(_localImages))
                .RuleFor(i => i.CreatedAt, f => f.Date.Past());

            await Task.Delay(1000);

            return generator.GenerateForever().Skip(page * perPage).Take(perPage);
        }
    }
}
