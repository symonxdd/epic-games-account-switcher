using System.IO;

namespace AccountSwitcher.Constants;

public static class Constants
{
  public const string BaseFolderName = "Epic Switcher (those who know)";
  public const string MappingsFileName = "username_id_mappings.txt";

  public static readonly string EpicGamesExeDir = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
    @"Epic Games\Launcher\Portal\Binaries\Win32\EpicGamesLauncher.exe");

  public static readonly string GameUserSettingsFile = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
      @"EpicGamesLauncher\Saved\Config\Windows\GameUserSettings.ini");

  public static readonly string GameUserSettingsDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        @"EpicGamesLauncher\Saved\Config\Windows");

  public static readonly string LogsDirectory = Path.Combine(
          Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
          @"EpicGamesLauncher\Saved\Logs");

  public static readonly string ActiveSessionsDir = Path.Combine(
          Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
          BaseFolderName, "Active Login Sessions");

  public static readonly string EpicSwitcherDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            BaseFolderName);

  public static readonly string MappingsFileDir = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    BaseFolderName, MappingsFileName);
}