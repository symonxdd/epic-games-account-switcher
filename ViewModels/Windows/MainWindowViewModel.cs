using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;

namespace AccountSwitcher.ViewModels.Windows {
  public partial class MainWindowViewModel : ObservableObject {

    [ObservableProperty]
    private string _applicationTitle = "Epic Switcher";

    [ObservableProperty]
    private ObservableCollection<object> _menuItems = [
      new NavigationViewItem() {
          Content = "Accounts",
          Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
          TargetPageType = typeof(Views.Pages.DashboardPage)
      },
      new NavigationViewItem() {
          Content = "Manage",
          Icon = new SymbolIcon { Symbol = SymbolRegular.Folder24 },
          TargetPageType = typeof(Views.Pages.ManagePage)
      }
    ];

    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems = [
      new Image {
        Source = new BitmapImage(new Uri("pack://application:,,,/Assets/capy_water_submerged.png")),
        Width = 135,
        HorizontalAlignment = HorizontalAlignment.Center
      },
      new NavigationViewItem() {
        Content = "How it works",
        Icon = new SymbolIcon { Symbol = SymbolRegular.Lightbulb24 },
        TargetPageType = typeof(Views.Pages.AboutPage)
      },
      new NavigationViewItem() {
        Content = "Settings",
        Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
        TargetPageType = typeof(Views.Pages.SettingsPage)
      }
    ];

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = [
      new MenuItem { Header = "Home", Tag = "tray_home" }
    ];
  }
}
