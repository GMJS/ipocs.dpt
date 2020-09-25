using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ipocs.dpt.Views
{
  public class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
      System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
      string version = fvi.FileVersion;
      Title += " - " + version;
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
