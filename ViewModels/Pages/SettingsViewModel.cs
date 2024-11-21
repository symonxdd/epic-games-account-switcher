using System.Diagnostics;
using AccountSwitcher.Resources;
using AccountSwitcher.Services.Interfaces;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace AccountSwitcher.ViewModels.Pages {
  public partial class SettingsViewModel : ObservableObject, INavigationAware {
    public IRelayCommand TestEpicReaderCommand { get; }
    private readonly IEpicService _epicService;
    private readonly IEpicLogReaderService _epicLogReaderService;

    private bool _isInitialized = false;

    [ObservableProperty]
    private bool _isFlyoutOpen;

    [ObservableProperty]
    private string _currentSentence;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

    public SettingsViewModel(IEpicService epicService, IEpicLogReaderService epicLogReaderService) {
      _epicLogReaderService = epicLogReaderService;
      _epicService = epicService;

      TestEpicReaderCommand = new RelayCommand(ExtractEpicLogDataAsync);
    }

    private void ExtractEpicLogDataAsync() {
      _epicLogReaderService.ExtractUsernameUserIdMappingsAsync();
    }

    public void OnNavigatedTo() {
      CurrentSentence = CapybaraSentences.GetRandomSentence();

      if (!_isInitialized)
        InitializeViewModel();
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel() {
      CurrentTheme = ApplicationThemeManager.GetAppTheme();
      AppVersion = $" (Epic Switcher v{GetAssemblyVersion()})";

      _isInitialized = true;
    }

    private string GetAssemblyVersion() {
      return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(3)
          ?? string.Empty;
    }

    [RelayCommand]
    private void OnChangeTheme(string parameter) {
      switch (parameter) {
        case "theme_light":
          if (CurrentTheme == ApplicationTheme.Light)
            break;

          ApplicationThemeManager.Apply(ApplicationTheme.Light);
          CurrentTheme = ApplicationTheme.Light;

          break;

        default:
          if (CurrentTheme == ApplicationTheme.Dark)
            break;

          ApplicationThemeManager.Apply(ApplicationTheme.Dark);
          CurrentTheme = ApplicationTheme.Dark;

          break;
      }
    }

    [RelayCommand]
    private void OnFolderClicked(string parameter) {
      switch (parameter) {
        case "user_settings":
          Process.Start("explorer.exe", Constants.Constants.GameUserSettingsDir);
          break;

        case "epic_switcher_dir":
          Process.Start("explorer.exe", Constants.Constants.EpicSwitcherDir);
          break;

        case "epic_logs":
          Process.Start("explorer.exe", Constants.Constants.LogsDirectory);
          break;
      }
    }
  }
}
