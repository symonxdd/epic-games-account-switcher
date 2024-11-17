using AccountSwitcher.Models;

namespace AccountSwitcher.Services.Interfaces
{
  public interface IEpicService
  {
    Task<string?> GetLoggedInUsername();
    Task<List<Session>> GetAllActiveLoginSessionsAsync();
    Task AddLoginAsync();
    Task SwitchAccountAsync(string selectedUsername);
  }
}
