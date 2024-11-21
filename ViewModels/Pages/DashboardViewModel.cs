using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using AccountSwitcher.Resources;
using AccountSwitcher.Services.Interfaces;
using Microsoft.VisualBasic;
using Wpf.Ui.Controls;

namespace AccountSwitcher.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject, INavigationAware {
  private readonly IEpicService _epicService;

  [ObservableProperty]
  private ObservableCollection<string> _myCollection;

  [ObservableProperty]
  private string? _selectedItem;

  [ObservableProperty]
  private string _listViewVisibility;

  [ObservableProperty]
  private string _infoBarMessage;

  [ObservableProperty]
  private string _currentSentence;

  [ObservableProperty]
  private InfoBarSeverity _myInfoBarSeverity;

  public DashboardViewModel(IEpicService epicService, IEpicLogReaderService epicLogReaderService) {
    _epicService = epicService;
    MyCollection = [];
  }

  public async Task LoadAccountStatusAsync() {
    MyCollection.Clear();

    // Check for logged-in status
    var username = await _epicService.GetLoggedInUsername();
    var _loggedInUsername = $"{username} (logged in)";

    if (username != "Logged Out") {
      MyCollection.Add($"{username} (logged in)"); // Add currently logged-in user with "(logged in)" suffix
    }

    // Add usernames from active login sessions if any
    var activeSessions = await _epicService.GetAllActiveLoginSessionsAsync();

    if (activeSessions.Count != 0) {
      foreach (var session in activeSessions) {
        // Check if the username already exists in the collection
        var existingItem = MyCollection.FirstOrDefault(item => item.Contains(session.Username));

        if (existingItem != null) {
          // If an alias is set and it's not the existing item, update it with the alias
          if (!string.IsNullOrWhiteSpace(session.Alias) && existingItem != session.Alias) {

            int index = MyCollection.IndexOf(existingItem);
            if (index != -1) {
              MyCollection[index] = $"{session.Alias} (logged in)";
              _loggedInUsername = $"{session.Alias} (logged in)";
            }
          }
        } else {
          // If the username is not found, add either the alias or the username
          var item = string.IsNullOrWhiteSpace(session.Alias) ? session.Username : session.Alias;
          MyCollection.Add(item);
        }
      }

      ListViewVisibility = "Visible";
      InfoBarMessage = "To add a new login, check the Manage tab";
      MyInfoBarSeverity = InfoBarSeverity.Informational;
    } else if (username == "Logged Out") {
      MyCollection.Add("(logged out)");
      ListViewVisibility = "Hidden";
      InfoBarMessage = "To start, log into the Epic Games Launcher, then check the Manage tab.";
      MyInfoBarSeverity = InfoBarSeverity.Warning;
    }

    // Set SelectedItem to the first item if MyCollection is not empty
    if (MyCollection.Any()) {
      SelectedItem = _loggedInUsername;
    }
  }

  public void OnNavigatedTo() {
    CurrentSentence = CapybaraSentences.GetRandomSentence();
  }

  public void OnNavigatedFrom() {
    //throw new NotImplementedException();
  }
}