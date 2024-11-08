namespace AccountSwitcher.Services.Interfaces
{
  public interface IEpicService
  {
    Task<bool> IsUserLoggedInAsync();
  }
}
