using System;
using System.Collections.Generic;
using System.Text;

namespace ipocs.dpt.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    public string Greeting => "Welcome to Avalonia!";

    public YardEditorViewModel YardEditorViewModel {get;} = new YardEditorViewModel();
  }
}
