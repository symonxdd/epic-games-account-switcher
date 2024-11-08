using System.IO;
using System.Text.RegularExpressions;
using AccountSwitcher.Services.Interfaces;

public class EpicService : IEpicService
{
  private readonly string _logFilePath = @"C:\Users\Symon\AppData\Local\EpicGamesLauncher\Saved\Config\Windows\GameUserSettings.ini";

  public async Task<bool> IsUserLoggedInAsync()
  {
    if (!File.Exists(_logFilePath)) return false;

    string logContent = await File.ReadAllTextAsync(_logFilePath);
    string pattern = @"Data=([^\r\n]+)";
    Match match = Regex.Match(logContent, pattern);

    if (match.Success && match.Groups[1].Value.Length >= 250)
      return true;

    return false;
  }
}
