using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData;
using ipocs.dpt.ViewModels;
using ipocs.objects.Objects;
using IPOCS.Protocol;
using IPOCS.Protocol.Packets.Orders;
using System.Collections.Generic;

namespace ipocs.dpt.Views
{
  public class UnitController : UserControl
  {
    public UnitController()
    {
      this.InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }

    private void ObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if ((e.Source as ComboBox).SelectedIndex == -1)
        return;
      var basicObject = (e.Source as ComboBox).SelectedItem as BasicObject;
      List<Packet> packets = basicObject switch
      {
        Booster _ => new List<Packet>(),
        GenericInput _ => new List<Packet>() { new SignOfLifeTimer() },
        GenericOutput _ => new List<Packet>() { new SignOfLifeTimer(), new SetOutput() },
        IRDetection _ => new List<Packet>(),
        LevelCrossing _ => new List<Packet>() { new SignOfLifeTimer(), new SetLevelCrossing() },
        objects.Objects.Points _ => new List<Packet>() { new SignOfLifeTimer(), new ThrowPoints() },
        SemaphoreWing _ => new List<Packet>(),
        SignalBoard _ => new List<Packet>(),
        Turntable _ => new List<Packet>(),
        _ => new List<Packet>(),
      };
      packets.Insert(0, new RequestStatus());
      (DataContext as ClientViewModel).Packets.Clear();
      (DataContext as ClientViewModel).Packets.AddRange(packets);
      var packetsList = this.FindControl<ComboBox>("PacketsList");
      packetsList.SelectedIndex = 0;
    }
  }
}
