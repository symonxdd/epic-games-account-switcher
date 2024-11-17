namespace AccountSwitcher.Services.Interfaces
{
  public interface IEpicLogReaderService
  {
    Task ExtractUsernameUserIdMappingsAsync();
  }
}
