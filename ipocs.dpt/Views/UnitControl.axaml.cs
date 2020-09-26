using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
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

    public void ToggleListenButtonClicked(object sender, RoutedEventArgs args)
    {
      IPOCS.Networker.Instance.isListening = !IPOCS.Networker.Instance.isListening;
    }
  }
}
