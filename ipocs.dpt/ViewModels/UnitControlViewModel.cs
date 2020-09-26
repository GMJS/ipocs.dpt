using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using IPOCS.Protocol.Packets.Orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection.Emit;
using System.Text;

namespace ipocs.dpt.ViewModels
{
  public class UnitControlViewModel: ViewModelBase
  {
    private MainWindowViewModel MainViewModel { get; }
    public bool Listening { get; private set; } = false;
    public ObservableCollection<ClientViewModel> Clients { get; } = new ObservableCollection<ClientViewModel>();
    public ObservableCollection<string> Log { get; } = new ObservableCollection<string>();

    public UnitControlViewModel(MainWindowViewModel mvm)
    {
      MainViewModel = mvm;
      IPOCS.Networker.Instance.OnConnect += Instance_OnConnect;
      IPOCS.Networker.Instance.OnConnectionRequest += Instance_OnConnectionRequest;
      IPOCS.Networker.Instance.OnDisconnect += Instance_OnDisconnect;
      IPOCS.Networker.Instance.OnListening += Instance_OnListening;
      var lifeTime = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime);
      lifeTime.Startup += (sender, e) =>
      {
        lifeTime.MainWindow.Closing += (s, e2) =>
        {
          IPOCS.Networker.Instance.isListening = false;
        };
      };
    }

    private void Instance_OnListening(bool isListening)
    {
      Listening = isListening;
    }

    private void Instance_OnDisconnect(IPOCS.Client client)
    {
      var clientModel = Clients.FirstOrDefault(c => c.Client == client);
      Clients.Remove(clientModel);
      Log.Insert(0, $"{DateTime.Now.ToUniversalTime()} : {client.Name} disconnected.");
    }

    private bool? Instance_OnConnectionRequest(IPOCS.Client client, IPOCS.Protocol.Packets.ConnectionRequest request)
    {
      var concentrator = MainViewModel.YardEditorViewModel.Concentrators.FirstOrDefault((c) => c.Name == client.Name);
      if (concentrator == null)
      {
        Log.Insert(0, $"{DateTime.Now.ToUniversalTime()} : Unable to locate {client.Name} - forcing disconnect.");
        return false;
      }
      List<byte> vector;
      try
      {
        vector = concentrator.Serialize();
      }
      catch (NullReferenceException)
      {
        Log.Insert(0, $"{DateTime.Now.ToUniversalTime()} : Serialization for {client.Name} site data failed.");
        return false;
      }

      ushort providedChecksum = ushort.Parse(request.RXID_SITE_DATA_VERSION);
      ushort computedChecksum = IPOCS.CRC16.Calculate(vector.ToArray());
      Log.Insert(0, $"{DateTime.Now.ToUniversalTime()} : ({client.Name}) R.CRC: {request.RXID_SITE_DATA_VERSION}, C.CRC: {computedChecksum}");
      if (providedChecksum == 0 || computedChecksum != providedChecksum)
      {
        var responseMsg = new IPOCS.Protocol.Message
        {
          RXID_OBJECT = client.Name
        };
        responseMsg.packets.Add(new IPOCS.Protocol.Packets.ApplicationData
        {
          RNID_XUSER = 0x0001,
          PAYLOAD = vector.ToArray()
        });
        client.Send(responseMsg);
        return false;
      }
      return true;
    }

    private void Instance_OnConnect(IPOCS.Client client)
    {
      var concentrator = MainViewModel.YardEditorViewModel.Concentrators.FirstOrDefault((c) => c.Name == client.Name);
      Clients.Add(new ClientViewModel(client, concentrator));
      Log.Insert(0, $"{DateTime.Now.ToUniversalTime()} : {client.Name} connected.");
    }
  }
}
