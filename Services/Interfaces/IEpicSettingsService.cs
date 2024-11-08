namespace AccountSwitcher.Services.Interfaces
{
  public interface IEpicSettingsService
  {
    Task<string> CheckAccountStatusAsync();
  }
}
