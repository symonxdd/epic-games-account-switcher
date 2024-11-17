using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace AccountSwitcher.ViewModels.Windows
{
  public partial class MainWindowViewModel : ObservableObject
  {
    [ObservableProperty]
    private string _applicationTitle = "Epic Switcher";

    [ObservableProperty]
    private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Accounts",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Aliases",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Tag24 },
                TargetPageType = typeof(Views.Pages.AliasesPage)
            }
        };

    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
  }
}
