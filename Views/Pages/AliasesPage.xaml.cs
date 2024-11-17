using AccountSwitcher.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace AccountSwitcher.Views.Pages
{
  public partial class AliasesPage : INavigableView<AliasesViewModel>
  {
    public AliasesViewModel ViewModel { get; }

    public AliasesPage(AliasesViewModel viewModel)
    {
      ViewModel = viewModel;
      DataContext = this;

      InitializeComponent();
    }
  }
}
