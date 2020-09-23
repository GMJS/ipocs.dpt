using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ipocs.dpt.Views
{
  public class UnitControl : UserControl
  {
    public UnitControl()
    {
      this.InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
