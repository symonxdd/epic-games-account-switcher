using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui.Controls;
using TextBlock = Wpf.Ui.Controls.TextBlock;

namespace AccountSwitcher.ViewModels.Pages {
  public partial class AboutViewModel : ObservableObject, INavigationAware {

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private string _accentColor;

    public AboutViewModel() {
      AppVersion = $"Epic Switcher v{GetAssemblyVersion()}";
    }

    private string GetAssemblyVersion() {
      return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(3)
          ?? string.Empty;
    }

    [RelayCommand]
    private async Task OnShowTermsOfUse(object sender) {
      var uiMessageBox = new Wpf.Ui.Controls.MessageBox {
        CloseButtonText = "Yea yea...",
        Title = "Terms of Use",
        Content = new StackPanel {
          Children = {
            new TextBlock {
              Inlines = {
                new Run { Text = "This open-source tool is provided " + "\"as is\"" + " without warranties of any kind. By using this tool, " },
                new Run { Text = "you agree that any outcomes or consequences arising from its use are your responsibility. " },
                new Run { Text = "The creators are not liable for any damages, " },
                new Run { Text = "losses, or issues that may occur." }
              },
              TextWrapping = TextWrapping.Wrap,
            }
          }
        },
      };
      _ = await uiMessageBox.ShowDialogAsync();
    }

    [RelayCommand]
    private async Task OnShowPrivacyPolicy(object sender) {
      var uiMessageBox = new Wpf.Ui.Controls.MessageBox {
        CloseButtonText = "Yea yea...",
        Title = "Privacy Policy",
        Content = new StackPanel {
          Children = {
            new TextBlock {
              Inlines = {
                new Run { Text = "This app does not collect, store, transfer, or share any user data. " },
                new Run { Text = "All file operations are performed locally on your device." }
              },
              TextWrapping = TextWrapping.Wrap,
            }
          }
        },
      };
      _ = await uiMessageBox.ShowDialogAsync();
    }

    public void OnNavigatedTo() {
      AccentColor = ((Color)Application.Current.Resources["SystemAccentColorPrimary"]).ToString();
    }

    public void OnNavigatedFrom() { }
  }
}
