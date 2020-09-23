using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ipocs.objects.Objects;
using PostSharp.Aspects.Advices;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Interactivity;
using PostSharp.Patterns.Model;
using ReactiveUI;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Reflection;
using System;

namespace ipocs.dpt.Views
{
  public class PropertyEditor : UserControl
  {
    public class Property
    {
      private readonly object source;
      private readonly PropertyInfo pi;
      public Property(PropertyInfo pi, object source)
      {
        this.pi = pi;
        this.source = source;
        Name = pi.Name;
        Value = pi.GetMethod.Invoke(source, null);
      }

      public string Name { get; }

      public object Value
      {
        get
        {
          return pi.GetMethod.Invoke(source, null);
        }
        set
        {
          pi.SetMethod?.Invoke(source, new[] { Convert.ChangeType(value, pi.PropertyType) });
        }
      }
    }

    public static readonly DirectProperty<PropertyEditor, object> ItemProperty =
      AvaloniaProperty.RegisterDirect<PropertyEditor, object>(nameof(Item), GetItem, SetItem);

    private object Item { get; set; }

    public AvaloniaList<Property> ItemProperties { get; } = new AvaloniaList<Property>();

    public PropertyEditor()
    {
      this.InitializeComponent();
      this.DataContext = this;
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }

    private static object GetItem(PropertyEditor pe)
    {
      return pe.Item;
    }

    private static void SetItem(PropertyEditor pe, object basicObject)
    {
      pe.ItemProperties.Clear();
      if (basicObject != null)
      {
        var properties = basicObject.GetType().GetProperties().OrderBy(o => o.Name);
        foreach (var property in properties)
        {
          if (!property.CanWrite && !(property.PropertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(property.PropertyType)))
            continue;
          pe.ItemProperties.Add(new Property(property, basicObject));
        }
      }
      pe.Item = basicObject;
    }

    public void OnClickCommand(IList binding)
    {
      var window = new CollectionEditorWindow
      {
        DataContext = binding
      };
      window.Show();
      // do something
    }
  }
}