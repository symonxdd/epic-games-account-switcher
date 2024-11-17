using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using AccountSwitcher.Models;
using AccountSwitcher.Services.Interfaces;
using Wpf.Ui.Controls;

namespace AccountSwitcher.ViewModels.Pages;

public partial class AliasesViewModel : ObservableObject, INavigationAware
{
  private readonly IEpicService _epicService;
  private const string BaseFolderName = "Epic Switcher (those who know)";

  [ObservableProperty]
  private ObservableCollection<Session> _myCollection;

  public AliasesViewModel(IEpicService epicService)
  {
    _epicService = epicService;
    MyCollection = [];
  }

  [RelayCommand]
  public async Task SaveAsync()
  {
    await SaveAliasesAsync();
  }

  public async void OnNavigatedTo()
  {
    MyCollection.Clear();

    // Add usernames from active login sessions if any
    var activeSessions = await _epicService.GetAllActiveLoginSessionsAsync();

    if (activeSessions.Count != 0)
    {
      foreach (var session in activeSessions)
      {
        MyCollection.Add(session);
      }
    }
  }

  public async Task SaveAliasesAsync()
  {
    string activeSessionsDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        BaseFolderName, "Active Login Sessions"
    );

    foreach (var session in MyCollection)
    {
      // Skip saving if alias matches the original username
      if (session.Alias == session.Username) continue;

      string matchedFileName = Directory.EnumerateFiles(activeSessionsDir, "*", SearchOption.TopDirectoryOnly)
          .FirstOrDefault(file => Path.GetFileName(file).Contains(session.Username, StringComparison.OrdinalIgnoreCase));

      if (matchedFileName == null) continue;

      // Find the file matching the username
      string oldFileName = Path.Combine(
          activeSessionsDir,
          matchedFileName
      );

      // Construct the new filename with the alias
      string newFileName = Path.Combine(
          activeSessionsDir,
          string.IsNullOrEmpty(session.Alias)
              ? $"GameUserSettings ({session.UserId}-{session.Username}).ini"
              : $"GameUserSettings ({session.UserId}-{session.Username}-{session.Alias}).ini"
      );

      File.Move(oldFileName, newFileName);
    }
  }

  public void OnNavigatedFrom() { }
}
