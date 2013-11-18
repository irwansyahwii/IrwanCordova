using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ReusableToolkits.Implementations;
using ReusableToolkits.Interfaces;

namespace CordovaCore.Modules
{
  public class Device : CordovaModuleBase, ICordovaModule
  {    
    protected IOSVersionInfo OSVersionInfo;
    protected ISettings Settings;
    protected ISystemLog SystemLog;
    private GuardUtility _guardUtil;

    public Device(
      IOSVersionInfo osVersionInfo
      , ISettings settings
      , ISystemLog systemLog)
    {

      _guardUtil = new GuardUtility( "Modules.Device" );

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

    protected virtual string GetDeviceInfo(string callbackId, ICordovaCallBack callback)
    {
      _guardUtil.GuardParamStringNotEmpty( callbackId, "callbackId", "GetDeviceInfo" );
      _guardUtil.GuardParamNotNull( callback, "callback", "GetDeviceInfo" );
      
      string resultTemplate = "uuid:'{0}',name:'{1}',platform:'{2}',version:'{3}.{4}',cordova:'{5}'";

      string result = "{" + string.Format(resultTemplate, UniqueId.ToString()
        , System.Environment.MachineName
        , "Windows"        
        , OSVersionInfo.MajorVersion
        , OSVersionInfo.MinorVersion
        , "2.1.0") + "}";

      callback.SuccessCallback( callbackId, false, result );

      return result;
    }

    public string Execute( string callbackId, ICordovaCallBack cordovaCallback, string action, string args, object result )
    {
      if( string.Compare( "getDeviceInfo", action, false ) == 0)
      {
        return GetDeviceInfo( callbackId, cordovaCallback );
      }
      return ThrowMemberNotFound( action );
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
