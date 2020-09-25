using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ipocs.dpt.ViewModels;
using ipocs.dpt.Views;
using Onova;
using Onova.Services;
using System.Runtime.InteropServices;

namespace ipocs.dpt
{
  public class App : Application
  {
#if !DEBUG
    UpdateManager manager;
#endif
    public override void Initialize()
    {
      AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
      if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
      {
        desktop.MainWindow = new MainWindow
        {
          DataContext = new MainWindowViewModel(),
        };

#if !DEBUG
        var os = string.Empty;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
          os = "win";
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
          os = "linux";
        }
        os += "-" + RuntimeInformation.OSArchitecture.ToString().ToLower();

        // Configure to look for packages in specified directory and treat them as zips
        manager = new UpdateManager(
            new GithubPackageResolver("ipocsmr", "ipocs.dpt", "*" + os + "*"),
            new ZipPackageExtractor());
        // Check for new version and, if available, perform full update and restart
        // Check for updates
        _ = manager.CheckForUpdatesAsync().ContinueWith(resultTask =>
        {
          var result = resultTask.Result;
          if (result.CanUpdate)
          {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
              MessageBox.Show(desktop.MainWindow, "There's a new version available. Do you want to download and install?", "New version", MessageBox.MessageBoxButtons.YesNo).ContinueWith((boxResult) =>
              {
                if (boxResult.Result != MessageBox.MessageBoxResult.Yes)
                {
                  return;
                }
                // Prepare an update by downloading and extracting the package
                // (supports progress reporting and cancellation)
                _ = manager.PrepareUpdateAsync(result.LastVersion).ContinueWith((prepareTask) =>
                {
                  // Launch an executable that will apply the update
                  // (can be instructed to restart the application afterwards)
                  manager.LaunchUpdater(result.LastVersion);

                  // Terminate the running application so that the updater can overwrite files
                  System.Environment.Exit(0);
                });
              });
            });
          }
          else
          {
            manager = null;
          }
        });
#endif
      }

      base.OnFrameworkInitializationCompleted();
    }
  }
}
