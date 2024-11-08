using System.Collections.ObjectModel;
using AccountSwitcher.Services.Interfaces;

namespace AccountSwitcher.ViewModels.Pages
{
  public partial class DashboardViewModel : ObservableObject
  {
    private readonly IEpicSettingsService _epicSettingsService;
    private readonly IEpicLogReaderService _epicLogReaderService;

    public DashboardViewModel(IEpicSettingsService epicSettingsService, IEpicLogReaderService epicLogReaderService)
    {
      _epicSettingsService = epicSettingsService;
      _epicLogReaderService = epicLogReaderService;
      MyCollection = new ObservableCollection<string>();

      TestEpicReaderCommand = new RelayCommand(OnTestEpicReader);

      LoadAccountStatusAsync();
    }

    [ObservableProperty]
    private ObservableCollection<string> _myCollection;

    // Add RelayCommand for the Button
    public IRelayCommand TestEpicReaderCommand { get; }

    private async Task LoadAccountStatusAsync()
    {
      string status = await _epicSettingsService.CheckAccountStatusAsync();
      MyCollection.Clear();
      MyCollection.Add(status);
    }

    // The method the command will execute
    private void OnTestEpicReader()
    {
      _epicLogReaderService.ExtractEpicLogDataAsync();
    }
  }
}
