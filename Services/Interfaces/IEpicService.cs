namespace AccountSwitcher.Services.Interfaces
{
  public interface IEpicService
  {
    Task<string?> GetLoggedInUsername();
    Task<List<string>> GetAllActiveLoginSessionsAsync();
    Task AddLoginAsync();
    Task SwitchAccountAsync(string selectedUsername);
  }
}
