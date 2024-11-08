using System.Collections.ObjectModel;
using AccountSwitcher.Services.Interfaces;

namespace AccountSwitcher.ViewModels.Pages
{
  public partial class DashboardViewModel : ObservableObject
  {
    private readonly IEpicService _epicService;
    private readonly IEpicLogReaderService _epicLogReaderService;

    public DashboardViewModel(IEpicService epicService, IEpicLogReaderService epicLogReaderService)
    {
      _epicService = epicService;
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
      bool status = await _epicService.IsUserLoggedInAsync();

      //MyCollection.Clear();
      //MyCollection.Add(status);
    }

    // The method the command will execute
    private void OnTestEpicReader()
    {
      _epicLogReaderService.ExtractEpicLogDataAsync();
    }
  }
}
