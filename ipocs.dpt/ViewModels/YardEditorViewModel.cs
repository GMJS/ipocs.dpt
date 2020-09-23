using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DynamicData;
using ipocs.objects;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace ipocs.dpt.ViewModels
{
  public class YardEditorViewModel : ViewModelBase
  {
    public AvaloniaList<Concentrator> Concentrators { get; } = new AvaloniaList<Concentrator>();

    public void NewUnit(string _)
    {
      var newUnit = new Concentrator
      {
        Name = "New unit " + (this.Concentrators.Count + 1).ToString()
      };
      Concentrators.Add(newUnit);
    }
  }
}
