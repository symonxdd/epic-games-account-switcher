using System.Collections.ObjectModel;
using AccountSwitcher.Services.Interfaces;

namespace AccountSwitcher.ViewModels.Pages
{
  public partial class DashboardViewModel : ObservableObject
  {
    private readonly IEpicService _epicService;

    [ObservableProperty]
    private ObservableCollection<string> _myCollection;

    [ObservableProperty]
    private string? _selectedItem;

    public DashboardViewModel(IEpicService epicService, IEpicLogReaderService epicLogReaderService)
    {
      _epicService = epicService;
      MyCollection = new ObservableCollection<string>();
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
  }
}
