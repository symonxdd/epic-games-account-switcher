using System.Collections.ObjectModel;
using AccountSwitcher.Services.Interfaces;
using Microsoft.VisualBasic;

namespace AccountSwitcher.ViewModels.Pages;

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
    MyCollection = [];
  }

  public async Task LoadAccountStatusAsync()
  {
    MyCollection.Clear();

    // Check for logged-in status
    var username = await _epicService.GetLoggedInUsername();
    var _loggedInUsername = $"{username} (logged in)";

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
        // Check if the username already exists in the collection
        var existingItem = MyCollection.FirstOrDefault(item => item.Contains(session.Username));

        if (existingItem != null)
        {
          // If an alias is set and it's not the existing item, update it with the alias
          if (!string.IsNullOrWhiteSpace(session.Alias) && existingItem != session.Alias)
          {

            int index = MyCollection.IndexOf(existingItem);
            if (index != -1)
            {
              MyCollection[index] = $"{session.Alias} (logged in)";
              _loggedInUsername = $"{session.Alias} (logged in)";
            }
          }
        }
        else
        {
          // If the username is not found, add either the alias or the username
          var item = string.IsNullOrWhiteSpace(session.Alias) ? session.Username : session.Alias;
          MyCollection.Add(item);
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
      SelectedItem = _loggedInUsername;
    }
  }
}