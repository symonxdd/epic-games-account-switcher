using System.IO;
using System.Text.RegularExpressions;
using AccountSwitcher.Services.Interfaces;

public class EpicSettingsService : IEpicSettingsService
{
  private readonly string _logFilePath = @"C:\Users\Symon\AppData\Local\EpicGamesLauncher\Saved\Config\Windows\GameUserSettings.ini";

  public async Task<string> CheckAccountStatusAsync()
  {
    if (!File.Exists(_logFilePath)) return "No account";

    string logContent = await File.ReadAllTextAsync(_logFilePath);
    string pattern = @"Data=([^\r\n]+)";
    Match match = Regex.Match(logContent, pattern);

    if (match.Success && match.Groups[1].Value.Length >= 250)
      return "Current account";

    return "No account";
  }
}
