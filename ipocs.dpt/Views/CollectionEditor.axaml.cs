using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ipocs.dpt.Views
{
  public class CollectionEditor : UserControl
  {
    public CollectionEditor()
    {
      this.InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }

    public void OnAddButtonClicked(object sender, RoutedEventArgs args)
    {
      var listType = DataContext.GetType();
      var items = new List<MenuItem>();
      var type = listType.GetGenericArguments().First();
      foreach (Type o in Assembly.GetAssembly(type).GetTypes().Where((t) => type.IsAssignableFrom(t) && !t.IsAbstract))
      {
        var menuItem = new MenuItem { Header = o.Name };
        menuItem.Tag = o;
        menuItem.Click += (sender, e) =>
        {
          object newO = Activator.CreateInstance((sender as MenuItem).Tag as Type);
          (this.FindControl<ListBox>("ItemsList").DataContext as IList).Add(newO);
          this.FindControl<ListBox>("ItemsList").SelectedItem = newO;
        };
        items.Add(menuItem);
      }
      
      (sender as Button).ContextMenu.Items = items;
      (sender as Button).ContextMenu.Open();
    }
  }
}
