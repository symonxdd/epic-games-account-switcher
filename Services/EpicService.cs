using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using AccountSwitcher.Models;
using AccountSwitcher.Services.Interfaces;

public class EpicService : IEpicService
{
  private const string BaseFolderName = "Epic Switcher (those who know)";
  private const string MappingsFileName = "username_id_mappings.txt";

  private readonly string EpicGamesExePath = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
      @"Epic Games\Launcher\Portal\Binaries\Win32\EpicGamesLauncher.exe");
  private readonly string _logFilePath = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
      @"EpicGamesLauncher\Saved\Config\Windows\GameUserSettings.ini");
  private readonly string _activeSessionsDir = Path.Combine(
          Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
          BaseFolderName, "Active Login Sessions"
      );

  private readonly string _mappingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), BaseFolderName, MappingsFileName);

  private async Task<bool> IsUserLoggedInAsync()
  {
    if (!File.Exists(_logFilePath)) return false;

    string logContent = await File.ReadAllTextAsync(_logFilePath);
    string pattern = @"Data=([^\r\n]+)";
    Match match = Regex.Match(logContent, pattern);

    if (match.Success && match.Groups[1].Value.Length >= 1000) // was 250
      return true;

    return false;
  }

  public async Task<string?> GetLoggedInUsername()
  {
    if (!File.Exists(_mappingsFilePath) || !File.Exists(_logFilePath) || !(await IsUserLoggedInAsync()))
      return "Logged Out";

    var mappings = await File.ReadAllLinesAsync(_mappingsFilePath);
    var logContent = await File.ReadAllTextAsync(_logFilePath);

    foreach (var line in mappings)
    {
      var match = Regex.Match(line, @"^(.+):\s+([a-f0-9]+)$");
      if (match.Success)
      {
        string username = match.Groups[1].Value;
        string userId = match.Groups[2].Value;

        if (logContent.Contains(userId))
        {
          Directory.CreateDirectory(_activeSessionsDir);

          string matchedFileName = Directory.EnumerateFiles(_activeSessionsDir, "*", SearchOption.TopDirectoryOnly)
                    .FirstOrDefault(file => Path.GetFileName(file).Contains(username, StringComparison.OrdinalIgnoreCase));

          string targetFilePath = Path.Combine(_activeSessionsDir, matchedFileName);

          File.Copy(_logFilePath, targetFilePath, overwrite: true);

          return username;
        }
      }
    }

    return "Logged Out"; // Indicate logged-out state if no match found
  }

  public async Task<List<Session>> GetAllActiveLoginSessionsAsync()
  {
    var sessions = new List<Session>();

    if (Directory.Exists(_activeSessionsDir))
    {
      var files = Directory.GetFiles(_activeSessionsDir, "GameUserSettings*.ini");

      foreach (var file in files)
      {
        string fileName = Path.GetFileNameWithoutExtension(file);
        string pattern = @"GameUserSettings\s\((?<userId>[^-]+)-(?<username>[^-]+)(?:-(?<alias>.+))?\)";
        var match = Regex.Match(fileName, pattern);

        if (match.Success)
        {
          string userId = match.Groups["userId"].Value;
          string username = match.Groups["username"].Value;
          string alias = match.Groups["alias"].Value;

          sessions.Add(new Session
          {
            UserId = userId,
            Username = username,
            Alias = alias ?? username
          });
        }
      }
    }

    return sessions;
  }

  private async Task CloseEpicGamesLauncher()
  {
    var processes = Process.GetProcessesByName("EpicGamesLauncher");
    if (processes.Length != 0)
    {
      var process = processes[0];
      process.Kill();
      await process.WaitForExitAsync();
    }
  }

  private async Task StartEpicGamesLauncher() => await Task.FromResult(Process.Start(EpicGamesExePath));

  public async Task AddLoginAsync()
  {
    await CloseEpicGamesLauncher();

    // Path where the `.ini` file should be copied
    string destinationFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        @"EpicGamesLauncher\Saved\Config\Windows\GameUserSettings.ini");

    if (File.Exists(destinationFilePath))
    {
      File.Delete(destinationFilePath);
    }

    await StartEpicGamesLauncher();
  }

  public async Task SwitchAccountAsync(string selectedUsername)
  {
    // Close Epic Games Launcher
    await CloseEpicGamesLauncher();

    // Path where the `.ini` file should be copied
    string destinationFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        @"EpicGamesLauncher\Saved\Config\Windows\GameUserSettings.ini");

    // Find the correct `.ini` file in "Active Login Sessions" based on the username
    string activeSessionsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Epic Switcher (those who know)", "Active Login Sessions");

    //var iniFileToCopy = Directory.GetFiles(activeSessionsFolder, $"GameUserSettings*({selectedUsername})*.ini").FirstOrDefault();
    var iniFileToCopy = Directory.GetFiles(activeSessionsFolder, "GameUserSettings*.ini")
                                  .FirstOrDefault(file => file.Contains(selectedUsername));

    if (iniFileToCopy == null)
    {
      throw new FileNotFoundException("No session file found for the selected username.");
      //return;
    }

    // Delete the existing GameUserSettings.ini file, if it exists
    if (File.Exists(destinationFilePath))
    {
      File.Delete(destinationFilePath);
    }

    // Copy the selected .ini file to the destination
    File.Copy(iniFileToCopy, destinationFilePath);

    // Restart Epic Games Launcher
    await StartEpicGamesLauncher();
  }
}