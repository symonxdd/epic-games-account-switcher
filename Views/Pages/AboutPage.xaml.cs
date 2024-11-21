using AccountSwitcher.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace AccountSwitcher.Views.Pages {
  public partial class AboutPage : INavigableView<AboutViewModel> {
    public AboutViewModel ViewModel { get; }

    public AboutPage(AboutViewModel viewModel) {
      ViewModel = viewModel;
      DataContext = this;

      InitializeComponent();
    }

    private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
        FileName = e.Uri.AbsoluteUri,
        UseShellExecute = true
      });
      e.Handled = true;
    }
  }
}
