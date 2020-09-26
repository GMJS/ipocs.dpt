using System;
using System.Collections.Generic;
using System.Text;

namespace ipocs.dpt.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    public YardEditorViewModel YardEditorViewModel { get; }

    public UnitControlViewModel UnitControlViewModel { get; }

    public MainWindowViewModel() {
      YardEditorViewModel = new YardEditorViewModel();
      UnitControlViewModel = new UnitControlViewModel(this);
    }
  }
}
