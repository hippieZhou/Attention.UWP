using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Dtos
{
    public class ExploreDto : BaseDto<ExploreDto>
    {
        public string Title { get; set; }
        public Brush Background { get; set; }
        public string Thumbnail { get; set; }

        public static IEnumerable<ExploreDto> GetFakeData()
        {
            return colors.Select(x => new ExploreDto
            {
                Background = CreateAcrylicBrush(x),
                Thumbnail = $"ms-appx:///Assets/Explore/Avatar0{random.Next(0, 5)}.png",
                Title = DateTime.Now.ToString()
            });
        }
    }
}
