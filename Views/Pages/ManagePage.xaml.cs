using AccountSwitcher.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace AccountSwitcher.Views.Pages
{
  public partial class ManagePage : INavigableView<ManageViewModel>
  {
    public ManageViewModel ViewModel { get; }

    public ManagePage(ManageViewModel viewModel)
    {
      ViewModel = viewModel;
      DataContext = this;

      InitializeComponent(); 
    }
  }
}
