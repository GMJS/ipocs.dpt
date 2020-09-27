using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ipocs.dpt.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ipocs.dpt.Views
{
  public class YardEditor : UserControl
  {
    public YardEditor()
    {
      this.InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }

    public void OnDeleteMenuItemClicked(object sender, RoutedEventArgs args)
    {
      var cList = this.FindControl<ListBox>("ConcentratorList");
      var list = (cList.DataContext as YardEditorViewModel).Concentrators as IList;
      list.Remove((sender as MenuItem).DataContext);
    }

    public void OnLoadButtonClicked(object sender, RoutedEventArgs args)
    {
      var openFile = new OpenFileDialog();
      IControl window = this;
      while (window != null && !(window is Window))
      {
        window = window.Parent;
      }
      
      openFile.ShowAsync(window as Window).ContinueWith((files) =>
      {
        foreach (string file in files.Result)
        {
          var types = (from lAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                       from lType in lAssembly.GetTypes()
                       where typeof(objects.Objects.BasicObject).IsAssignableFrom(lType)
                       select lType).ToList();
          var types2 = (from lAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                        from lType in lAssembly.GetTypes()
                        where typeof(objects.Objects.PointsMotor).IsAssignableFrom(lType)
                        select lType).ToList();
          types.AddRange(types2);
          types.Add(typeof(objects.Objects.BasicObject));
          XmlSerializer xsSubmit = new XmlSerializer(typeof(List<objects.Concentrator>), types.ToArray());
          using var reader = XmlReader.Create(file);
          var objs = xsSubmit.Deserialize(reader) as List<objects.Concentrator>;
          Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
          {
            (DataContext as YardEditorViewModel).Concentrators.Clear();
            (DataContext as YardEditorViewModel).Concentrators.AddRange(objs);
          });
        }
      });
    }

    public void OnSaveButtonClicked(object sender, RoutedEventArgs args)
    {
      var sfd = new SaveFileDialog
      {
        DefaultExtension = "*.xml"
      };
      IControl window = this;
      while (window != null && !(window is Window))
      {
        window = window.Parent;
      }
      sfd.ShowAsync(window as Window).ContinueWith(async files =>
      {
        var saveFileName = files.Result;

        if (string.IsNullOrWhiteSpace(saveFileName))
        {
          return;
        }

        var types = (from lAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                     from lType in lAssembly.GetTypes()
                     where typeof(objects.Objects.BasicObject).IsAssignableFrom(lType)
                     select lType).ToList();
        var types2 = (from lAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                      from lType in lAssembly.GetTypes()
                      where typeof(objects.Objects.PointsMotor).IsAssignableFrom(lType)
                      select lType).ToList();
        types.AddRange(types2);
        types.Add(typeof(objects.Objects.BasicObject));
        XmlSerializer xsSubmit = new XmlSerializer(typeof(List<objects.Concentrator>), types.ToArray());
        using XmlWriter writer = XmlWriter.Create(saveFileName, new XmlWriterSettings
        {
          Indent = true,
          IndentChars = "  "
        });
        var concentrators = await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => (DataContext as YardEditorViewModel).Concentrators.ToList());
        xsSubmit.Serialize(writer, concentrators);
      });
    }
  }
}
