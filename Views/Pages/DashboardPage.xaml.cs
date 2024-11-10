using System.Diagnostics;
using System.Windows.Controls;
using AccountSwitcher.Services.Interfaces;
using AccountSwitcher.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace AccountSwitcher.Views.Pages
{
  public partial class DashboardPage : INavigableView<DashboardViewModel>
  {
    private readonly IEpicService _epicService;
    private bool _isProgrammaticChange = true;

    public DashboardViewModel ViewModel { get; }

    public DashboardPage(DashboardViewModel viewModel, IEpicService epicService)
    {
      ViewModel = viewModel;
      _epicService = epicService;
      DataContext = this;

      InitializeComponent();

      Loaded += DashboardPage_Loaded;
      Unloaded += DashboardPage_Unloaded;
    }

    private async void DashboardPage_Loaded(object sender, RoutedEventArgs e)
    {
      // Subscribe to the application's Activated event when the page is loaded
      Application.Current.Activated += OnAppActivated;

      _isProgrammaticChange = true;
      await ViewModel.LoadAccountStatusAsync();
      _isProgrammaticChange = false;
    }

    private void DashboardPage_Unloaded(object sender, RoutedEventArgs e)
    {
      // Unsubscribe to avoid memory leaks
      Application.Current.Activated -= OnAppActivated;
    }

    private async void OnAppActivated(object? sender, EventArgs e)
    {
      // Ensure this only runs when the DashboardPage is visible
      if (IsVisible)
      {
        // Call LoadAccountStatusAsync when the window is refocused and the page is visible
        _isProgrammaticChange = true;
        await ViewModel.LoadAccountStatusAsync();
        _isProgrammaticChange = false;
      }
    }

    private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var selectedUsername = (sender as ComboBox)?.SelectedItem as string;

      // Remove "(logged in)" suffix if it exists
      if (!string.IsNullOrEmpty(selectedUsername) && selectedUsername.EndsWith(" (logged in)"))
      {
        selectedUsername = selectedUsername.Substring(0, selectedUsername.Length - " (logged in)".Length);
      }

      // Prevent triggering selection change during programmatic change
      if (!_isProgrammaticChange)
      {
        await _epicService.SwitchAccountAsync(selectedUsername);

        // Update ComboBox to show the selected item with "(logged in)" suffix
        //UpdateComboBoxWithLoggedInStatus(selectedUsername);
        //Debug.WriteLine("WHEW WHEW WHEW");
      }
    }
  }
}