using Prism.Windows.AppModel;

namespace Attention.App.ViewModels.UcViewModels
{
    public class PickedSearchViewModel: PickedPaneViewModel
    {
        public PickedSearchViewModel(IResourceLoader resourceLoader) : base(PaneTypes.Search, resourceLoader.GetString("picked_Search"))
        {
        }
    }
}
