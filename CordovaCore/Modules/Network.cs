using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CordovaCore.Modules
{
  public class Network : ICordovaModule
  {
    #region ICordovaModule Members

    public void Execute( string callbackId, ICordovaCallBack cordovaCallback, string action, string args, object result )
    {
      if( action.Equals( "getConnectionInfo" ) )
      {
        GetNetworkInterfaceType( callbackId, cordovaCallback );
      }      
    }

    private static void GetNetworkInterfaceType( string callbackId, ICordovaCallBack cordovaCallback )
    {
      string networkType = "unknown";
      NetworkInterface[] ifaceList = NetworkInterface.GetAllNetworkInterfaces();
      foreach( NetworkInterface iface in ifaceList )
      {
        if( iface.OperationalStatus == OperationalStatus.Up )
        {
          switch( iface.NetworkInterfaceType )
          {
            case NetworkInterfaceType.Wireless80211:
              {
                networkType = "wifi";
              }
              break;
            case NetworkInterfaceType.Ethernet:
              {
                networkType = "ethernet";
              }
              break;
            default:
              networkType = "unknown";
              break;
          }
          break;
          //Console.WriteLine( "Name\t\t: " + iface.Name );
          //Console.WriteLine( "Type\t\t: " + iface.NetworkInterfaceType );
          //Console.WriteLine( "Status\t\t: " + iface.OperationalStatus );
          //Console.WriteLine( "Speed\t\t: " + iface.Speed );
          //Console.WriteLine( "Description\t: " + iface.Description );

          //UnicastIPAddressInformationCollection unicastIPC = iface.GetIPProperties().UnicastAddresses;
          //foreach( UnicastIPAddressInformation unicast in unicastIPC )
          //{
          //  Console.WriteLine( unicast.Address.AddressFamily + "\t: " + unicast.Address );
          //}
          //Console.WriteLine( "=======================================" );
        }
      }
      networkType = string.Format( "'{0}'", networkType );
      cordovaCallback.SuccessCallback( callbackId, false, networkType );
    }

    public void Initialize()
    {
      
    }

    #endregion
  }
}
