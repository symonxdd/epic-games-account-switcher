using System.Collections.ObjectModel;
using AccountSwitcher.Services.Interfaces;

namespace AccountSwitcher.ViewModels.Pages
{
  public partial class DashboardViewModel : ObservableObject
  {
    private readonly IEpicService _epicService;
    private readonly IEpicLogReaderService _epicLogReaderService;

    public IRelayCommand TestEpicReaderCommand { get; }
    public IRelayCommand AddLoginCommand { get; }
    public IRelayCommand OpenFlyoutCommand { get; }

    [ObservableProperty]
    private ObservableCollection<string> _myCollection;

    [ObservableProperty]
    private string? _selectedItem;

    [ObservableProperty]
    private bool _isFlyoutOpen;

    public DashboardViewModel(IEpicService epicService, IEpicLogReaderService epicLogReaderService)
    {
      _epicService = epicService;
      _epicLogReaderService = epicLogReaderService;
      MyCollection = new ObservableCollection<string>();

      TestEpicReaderCommand = new RelayCommand(OnTestEpicReader);
      AddLoginCommand = new RelayCommand(OnAddLogin);
      OpenFlyoutCommand = new RelayCommand(OnOpenFlyout);
    }

    public async Task LoadAccountStatusAsync()
    {
      MyCollection.Clear();

      // Check for logged-in status
      var username = await _epicService.GetLoggedInUsername();

      if (username != "Logged Out")
      {
        MyCollection.Add($"{username} (logged in)"); // Add currently logged-in user with "(logged in)" suffix
      }

      // Add usernames from active login sessions if any
      var activeSessions = await _epicService.GetAllActiveLoginSessionsAsync();

      if (activeSessions.Count != 0)
      {
        foreach (var session in activeSessions)
        {
          if (!MyCollection.Any(item => item.Contains(session))) // Ensure distinct usernames
          {
            MyCollection.Add(session); // Add without extra text
          }
        }
      }
      else if (username == "Logged Out") // Show "(logged out)" only if no active sessions and no user logged in
      {
        MyCollection.Add("(logged out)");
      }

      // Set SelectedItem to the first item if MyCollection is not empty
      if (MyCollection.Any())
      {
        SelectedItem = MyCollection[0];
      }
    }

    private async void OnAddLogin()
    {
      await _epicService.AddLoginAsync();
    }

    private void OnOpenFlyout()
    {
      IsFlyoutOpen = true;

      // previously...
      //var mainWindow = (MainWindow)Application.Current.MainWindow;
      //mainWindow.ShowAddLoginDialogAsync();
    }

    private void OnTestEpicReader()
    {
      _epicLogReaderService.ExtractEpicLogDataAsync();
    }
  }
}
