using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;
using AccountSwitcher.Services.Interfaces;
using AccountSwitcher.ViewModels.Windows;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using TextBlock = Wpf.Ui.Controls.TextBlock;

namespace AccountSwitcher.Views.Windows
{
  public partial class MainWindow : INavigationWindow
  {
    public MainWindowViewModel ViewModel { get; }
    private readonly IEpicLogReaderService _epicLogReaderService;

    public MainWindow(
        MainWindowViewModel viewModel,
        IPageService pageService,
        INavigationService navigationService,
        IEpicLogReaderService epicLogReaderService
    )
    {
      ViewModel = viewModel;
      DataContext = this;

      SystemThemeWatcher.Watch(this);

      InitializeComponent();
      SetPageService(pageService);

      navigationService.SetNavigationControl(RootNavigation);

      _epicLogReaderService = epicLogReaderService;
      CheckFirstRun();
    }

    private void CheckFirstRun()
    {
      //#if DEBUG
      //      Properties.Settings.Default.IsFirstRun = true;
      //      Properties.Settings.Default.Save();
      //#endif

      if (Properties.Settings.Default.IsFirstRun)
      {
        _epicLogReaderService.ExtractUsernameUserIdMappingsAsync();

        Properties.Settings.Default.IsFirstRun = false;
        Properties.Settings.Default.Save();
      }
    }

    public async Task ShowAddLoginDialogAsync()
    {
      ContentDialog contentDialog = new()
      {
        Title = "Add new login?",
        CloseButtonText = "Cancel",
        PrimaryButtonText = "Add Login",
        DialogMaxWidth = 500,
        DialogMaxHeight = 200,
        DialogHost = RootContentDialog, // Setting the dialog container
        Content = new StackPanel
        {
          Children =
          {
            new TextBlock
            {
              Inlines =
              {
                new Run { Text = "This will close the Epic Games Launcher. You will" },
                new Run { Text = " not ", FontWeight = FontWeights.Bold },
                new Run { Text = "be logged out; instead, the login session is persisted." }
              },
              TextWrapping = TextWrapping.Wrap,
            },
            new TextBlock
            {
              Text = "After you log into your other account, come back to this app to see it. " +
                      "You'll then be able to switch between them! 🥭",
              TextWrapping = TextWrapping.Wrap,
              Margin = new Thickness(0, 15, 0, 0)
            }
          }
        }
      };

      var result = await contentDialog.ShowAsync();
      if (result == ContentDialogResult.Primary)
      {
        Debug.WriteLine("clicked Primary button on dialog!");
        // Perform any actions for the "Add Login" primary button click
      }
    }

    #region INavigationWindow methods

    public INavigationView GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    #endregion INavigationWindow methods

    /// <summary>
    /// Raises the closed event.
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);

      // Make sure that closing this window will begin the process of closing the application.
      Application.Current.Shutdown();
    }

    INavigationView INavigationWindow.GetNavigation()
    {
      throw new NotImplementedException();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
      throw new NotImplementedException();
    }
  }
}
