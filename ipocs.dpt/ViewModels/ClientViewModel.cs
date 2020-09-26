using ipocs.objects;
using IPOCS;
using IPOCS.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ipocs.dpt.ViewModels
{
  public class ClientViewModel: ViewModelBase
  {
    public Client Client { get; }
    public Concentrator Concentrator { get; }
    public ipocs.objects.Objects.BasicObject SelectedObject { get; set; }
    public ObservableCollection<string> Log { get; } = new ObservableCollection<string>();
    public ObservableCollection<Packet> Packets { get; } = new ObservableCollection<Packet>();

    public ClientViewModel(Client client, Concentrator concentrator)
    {
      Client = client;
      Client.OnMessage += Client_OnMessage;
      Concentrator = concentrator;
    }

    private void Client_OnMessage(Message msg)
    {
      foreach (var packet in msg.packets)
      {
        Log.Insert(0, $"{DateTime.Now.ToUniversalTime()} : -<- {msg.RXID_OBJECT} : {packet.GetType().Name} : {packet}");
      }
    }

    public void SendPacket(Packet pkt)
    {
      var msg = new Message
      {
        RXID_OBJECT = SelectedObject.Name
      };
      Log.Insert(0, $"{DateTime.Now.ToUniversalTime()} : ->- {msg.RXID_OBJECT} : {pkt.GetType().Name} : {pkt}");
      msg.packets.Add(pkt);
      Client.Send(msg);
    }
  }
}
