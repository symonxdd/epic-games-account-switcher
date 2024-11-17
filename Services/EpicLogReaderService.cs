using System.IO;
using System.Text.RegularExpressions;
using AccountSwitcher.Services.Interfaces;

namespace AccountSwitcher.Services
{
  internal class EpicLogReaderService : IEpicLogReaderService
  {
    // Can freely be changed
    private const string BaseFolderName = "Epic Switcher (those who know)";

    // Path to the logs directory (example path, adjust as necessary)
    private readonly string logsDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        @"EpicGamesLauncher\Saved\Logs");

    // Define the directory path in AppData (LocalApplicationData)
    private readonly string appDataFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        BaseFolderName
    );

    string activeSessionsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Epic Switcher (those who know)", "Active Login Sessions");

    // Ensure the directory exists
    private readonly string outputFilePath;

    // Regular expression to match epicusername and epicuserid in log lines
    private readonly Regex logPattern = new Regex(
        @"-epicusername=""([^""]+)""(?:.*?)-epicuserid=([a-f0-9\-]+)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled
    );

    // HashSet to store unique pairs of epicusername and epicuserid
    private readonly HashSet<(string username, string userId)> _uniquePairs = new();

    public EpicLogReaderService()
    {
      // Ensure the directory exists
      Directory.CreateDirectory(appDataFolder);

      // Define the file path
      outputFilePath = Path.Combine(appDataFolder, "username_id_mappings.txt");
    }

    public async Task ExtractUsernameUserIdMappingsAsync()
    {
      // Find all log files with 'EpicGamesLauncher' in the filename
      var logFiles = Directory.GetFiles(logsDirectory, "*EpicGamesLauncher*.log")
                              .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                              .Take(3) // Only take the n most recent files
                              .ToList();

      // Guard clause: If no log files found, exit early
      if (logFiles.Count == 0)
      {
        Console.WriteLine("No log files found.");
        return; // Exit early
      }

      // Copy the most recent log file to the appDataFolder temporarily
      string recentLogFile = logFiles[0];
      string tempLogFile = Path.Combine(appDataFolder, Path.GetFileName(recentLogFile));

      try
      {
        // Copy the most recent log file to appDataFolder (only for the first file)
        File.Copy(recentLogFile, tempLogFile, overwrite: true);

        // Process the copied file (most recent one)
        await ProcessLogFileAsync(tempLogFile);

        // After processing, delete the copied file
        File.Delete(tempLogFile);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error processing the most recent log file: {ex.Message}");
      }

      // Process the remaining log files (including the already copied file)
      foreach (var logFile in logFiles.Skip(1)) // Skip the first (most recent) as it's already processed
      {
        await ProcessLogFileAsync(logFile);
      }

      // After processing the logs, save the unique pairs to the file
      SaveUniquePairsToFile();
    }

    private async Task ProcessLogFileAsync(string logFile)
    {
      try
      {
        // Open the log file for reading line by line
        using var reader = new StreamReader(logFile);

        string line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
          // Match the line with the regex to find epicusername and epicuserid
          var match = logPattern.Match(line);
          if (match.Success)
          {
            var username = match.Groups[1].Value;
            var userId = match.Groups[2].Value;

            // If this pair is unique, log it and add it to the HashSet
            if (_uniquePairs.Add((username, userId)))
            {
              // Log the unique username and ID to the console (for now)
              Console.WriteLine($"Found User: {username}, UserID: {userId}");
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error processing file {logFile}: {ex.Message}");
      }
    }

    // This method will save all the unique username-userID pairs to a file without duplicates
    private void SaveUniquePairsToFile()
    {
      try
      {
        // Load existing mappings into a dictionary (userId -> username)
        var existingMappings = new Dictionary<string, string>();

        if (File.Exists(outputFilePath))
        {
          foreach (var line in File.ReadLines(outputFilePath))
          {
            var parts = line.Split(": ");
            if (parts.Length == 2)
            {
              existingMappings[parts[1]] = parts[0]; // userId -> username
            }
          }
        }

        // Update existing mappings with _uniquePairs, renaming files if username changes
        foreach (var (username, userId) in _uniquePairs)
        {
          if (existingMappings.TryGetValue(userId, out var oldUsername) && oldUsername != username)
          {
            // Find and rename the file containing the old username
            foreach (var filePath in Directory.GetFiles(activeSessionsFolder))
            {
              var fileName = Path.GetFileName(filePath);
              if (fileName.Contains($"({userId}-{oldUsername})"))
              {
                var newFileName = fileName.Replace($"({userId}-{oldUsername})", $"({userId}-{username})");
                var newFilePath = Path.Combine(activeSessionsFolder, newFileName);
                File.Move(filePath, newFilePath);
                break; // Rename only one matching file
              }
            }
          }
          existingMappings[userId] = username;
        }

        // Write all mappings back to the file
        using var writer = new StreamWriter(outputFilePath, append: false);
        foreach (var (userId, username) in existingMappings)
        {
          writer.WriteLine($"{username}: {userId}");
        }

        Console.WriteLine($"Saved {_uniquePairs.Count} unique user mappings to file.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error saving data to file: {ex.Message}");
      }
    }
  }
}
