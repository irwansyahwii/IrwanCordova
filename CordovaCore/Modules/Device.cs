using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ReusableToolkits.Interfaces;

namespace CordovaCore.Modules
{
  public class Device : ICordovaModule
  {
    protected ICordovaCallBack Callback;
    protected IOSVersionInfo OSVersionInfo;
    protected ISettings Settings;
    protected ISystemLog SystemLog;

    public Device( ICordovaCallBack callback
      , IOSVersionInfo osVersionInfo
      , ISettings settings
      , ISystemLog systemLog)
    {
      Callback = callback;
      OSVersionInfo = osVersionInfo;
      Settings = settings;
      SystemLog = systemLog;
    }

    public Guid UniqueId { get; set; }

    #region ICordovaModule Members

    public void Initialize()
    {      
      AcquireUniqueId();

      SystemLog.LogInfo( "Device module initialized" );
    }

    protected virtual void GetDeviceInfo(string callbackId)
    {
      string resultTemplate = "uuid:'{0}',name:'{1}',platform:'{2}',version:'{3}.{4}',cordova:'{5}'";

      string result = "{" + string.Format(resultTemplate, UniqueId.ToString()
        , System.Environment.MachineName
        , "Windows"        
        , OSVersionInfo.MajorVersion
        , OSVersionInfo.MinorVersion
        , "2.1.0") + "}";

      Callback.SuccessCallback( callbackId, false, result );
    }

    public void Execute( string callbackId, string action, string args, object result )
    {
      if( string.Compare( "getDeviceInfo", action, false ) == 0)
      {
        GetDeviceInfo( callbackId );
      }
    }

    #endregion

    public static Guid DummyGuid = Guid.Empty;

    protected virtual Guid GetNewGuid()
    {
      if( DummyGuid != Guid.Empty )
      {
        return DummyGuid;
      }

      return Guid.NewGuid();
    }

    protected void AcquireUniqueId()
    {
      UniqueId = GetNewGuid();
      using( var cordovaKey = Registry.CurrentUser.CreateSubKey( @"Software\Intel\Cordova", RegistryKeyPermissionCheck.ReadWriteSubTree ) )
      {
        if( cordovaKey != null )
        {
          object existingValue = cordovaKey.GetValue( "MachineID" );
          if( existingValue != null )
          {
            Guid existingGuid = Guid.Parse( existingValue.ToString() );
            if( Guid.Empty != existingGuid )
            {
              UniqueId = existingGuid;
              return;
            }
            else
            {
              cordovaKey.SetValue( "MachineID", UniqueId );
            }

          }
          else
          {
            cordovaKey.SetValue( "MachineID", UniqueId );
          }          
        }               
      }
    }
  }
}
