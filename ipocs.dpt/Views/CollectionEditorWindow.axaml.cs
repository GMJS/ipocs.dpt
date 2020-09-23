using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Xml;

namespace ipocs.dpt.Views
{
  public class CollectionEditorWindow : Window
  {
    public CollectionEditorWindow()
    {
      this.InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }

    public void OnClose()
    {
      Close();
    }
  }
}
