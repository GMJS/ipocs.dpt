using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ipocs.dpt.Views
{
  public class ConcentratorEditor : UserControl
  {
    public ConcentratorEditor()
    {
      this.InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
