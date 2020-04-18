using Prism.Commands;
using Prism.Windows.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System.Linq;

namespace Attention.App.ViewModels
{
    public class CategoryBase { }

    public class Category : CategoryBase
    {
        public string Name { get; set; }
        public string Tooltip { get; set; }
        public Symbol Glyph { get; set; }
    }

    public class Separator : CategoryBase { }

    public class Header : CategoryBase
    {
        public string Name { get; set; }
    }


    public class ShellPageViewModel: ViewModelBase
    {
        public ShellPageViewModel()
        {

        }

        private string _header = "首页";
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        private ObservableCollection<CategoryBase> _categories;
        public ObservableCollection<CategoryBase> Categories
        {
            get { return _categories ?? (_categories = new ObservableCollection<CategoryBase>()); }
            set { SetProperty(ref _categories, value); }
        }

        private CategoryBase _selectedItem;
        public CategoryBase SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(() => 
                    {
                        Categories.Clear();
                        Categories.Add(new Separator());
                        Categories.Add(new Header() { Name = "操作" });
                        Categories.Add(new Category { Name = "首页", Glyph = Symbol.Home, Tooltip = "首页" });
                        Categories.Add(new Category { Name = "下载", Glyph = Symbol.Download, Tooltip = "下载" });
                        SelectedItem = Categories.FirstOrDefault(x => x.GetType() == typeof(Category));
                    });
                }
                return _loadCommand;
            }
        }
    }
}
