using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaCore.Modules;
using Microsoft.Win32;
using NUnit.Framework;
using TechTalk.SpecFlow;
using IrwanUnitTestFramework;
using Rhino.Mocks;
using CordovaCore;
using ReusableToolkits.Interfaces;

namespace CordovaTests.Tests.Modules
{  
  [Binding]
  public class DeviceModuleTests
  {
    protected string GuidString;
    [Given( @"This GUID ""(.*)""" )]
    public void GivenThisGUID( string p0 )
    {
      //ScenarioContext.Current.Pending();
      GuidString = p0.ToLower();
    }

    [Given( @"Machine id key is not exists" )]
    public void GivenMachineIdKeyIsNotExists()
    {      
      using( var key = Registry.CurrentUser.OpenSubKey( @"Software\Intel\Cordova" ) )
      {

        if( key != null )
        {
          Registry.CurrentUser.DeleteSubKey( @"Software\Intel\Cordova" );
        }

      }

      using( var key = Registry.CurrentUser.OpenSubKey( @"Software\Intel\Cordova" ) )
      {
        Assert.IsTrue( key == null );
      }
      
    }

    protected Device Device;
    [When( @"Device\.AcquireUniqueId\(\) is called" )]
    public void WhenDevice_AcquireUniqueIdIsCalled()
    {
      Device = new Device( null, null, null);
      Device.DummyGuid = Guid.Parse(GuidString);
      Device.Call( "AcquireUniqueId", null );
    }

    [Then( @"A new key with name MachineId must be created in registry \\CurrentUser\\Software\\Intel\\Cordova" )]
    public void ThenANewKeyWithNameMachineIdMustBeCreatedInRegistryCurrentUserSoftwareIntelCordova()
    {
      using( var key = Registry.CurrentUser.OpenSubKey( @"Software\Intel\Cordova" ) )
      {
        var machineIdKey = key.GetValue( "MachineID" );
        Assert.IsTrue( key != null );
      }
    }

    [Then( @"it's value must be set to ""(.*)""" )]
    public void ThenItSValueMustBeSetTo( string p0 )
    {
      using( var key = Registry.CurrentUser.OpenSubKey( @"Software\Intel\Cordova" ) )
      {
        var machineIdKey = key.GetValue( "MachineID" );
        Assert.AreEqual( GuidString, machineIdKey );
      }
    }

    Guid ExistingGuid = Guid.Empty;

    [Given( @"MachineID key is exists with this GUID ""(.*)""" )]
    public void GivenMachineIDKeyIsExistsWithThisGUID( string p0 )
    {
      ExistingGuid = Guid.Parse( p0 );

      //delete the previous key
      using( var key = Registry.CurrentUser.OpenSubKey( @"Software\Intel\Cordova" ) )
      {
        if( key != null )
        {
          Registry.CurrentUser.DeleteSubKey( @"Software\Intel\Cordova" );
        }
      }

      //added a new key with the specified value
      using( var key = Registry.CurrentUser.OpenSubKey( @"Software\Intel\Cordova" ) )
      {
        if( key == null )
        {
          using( var cordovaKey = Registry.CurrentUser.CreateSubKey( @"Software\Intel\Cordova" ) )
          {
            cordovaKey.SetValue( "MachineID", ExistingGuid );
          }
        }
      }

      //make sure the add is a success
      using( var key = Registry.CurrentUser.OpenSubKey( @"Software\Intel\Cordova" ) )
      {
        Assert.IsNotNull( key );

        ExistingGuid = Guid.Parse( key.GetValue( "MachineID" ).ToString() );
        Assert.AreNotEqual( Guid.Empty, ExistingGuid );
      }
    }

    [Then( @"Existing MachineID must be the same as ""(.*)""" )]
    public void ThenExistingMachineIDMustBeTheSameAs( string p0 )
    {
      using( var key = Registry.CurrentUser.OpenSubKey( @"Software\Intel\Cordova" ) )
      {
        Assert.IsNotNull( key );

        Guid actualResult = Guid.Parse( key.GetValue( "MachineID" ).ToString() );
        Guid existingGuid = Guid.Parse( p0 );
        Assert.AreEqual( existingGuid, actualResult );        
      }
      
    }

    [Then( @"The Device\.UniqueId property value must be the same as ""(.*)""" )]
    public void ThenTheDevice_UniqueIdPropertyValueMustBeTheSameAs( string p0 )
    {
      Guid existingGuid = Guid.Parse( p0 );      
      Assert.AreEqual( existingGuid, Device.UniqueId );
    }


    ICordovaCallBack cordovaCallback = null;
    [When( @"Device\.Execute is called" )]
    public void WhenDevice_ExecuteIsCalled()
    {
      cordovaCallback = MockRepository.GenerateMock<ICordovaCallBack>();
      IOSVersionInfo osVersionInfo = MockRepository.GenerateMock<IOSVersionInfo>();
      ISettings settings = MockRepository.GenerateMock<ISettings>();

      osVersionInfo.Stub(x => x.MajorVersion).Return(2);
      osVersionInfo.Stub(x => x.MinorVersion).Return(1);


      Device = MockRepository.GeneratePartialMock<Device>( osVersionInfo, settings, MockRepository.GenerateMock<ISystemLog>());

      string resultTemplate = "uuid:'{0}',name:'{1}',platform:'{2}',version:'{3}.{4}',cordova:'{5}'";

      string result = "{" + string.Format( resultTemplate, Device.UniqueId.ToString()
        , System.Environment.MachineName
        , "Windows"
        , osVersionInfo.MajorVersion
        , osVersionInfo.MinorVersion
        , "2.1.0" ) + "}";

      cordovaCallback.Expect( x => x.SuccessCallback( callbackId, false, result ) );

      Device.Expect( x => x.Call( "GetDeviceInfo", new object[] { callbackId, cordovaCallback } ) ).CallOriginalMethod();
      Device.Execute( "1", cordovaCallback, action, string.Empty, string.Empty );

    }

    string action = string.Empty;
    string callbackId = string.Empty;
    [Given(@"This action ""(.*)"" and callback id ""(.*)""")]
    public void GivenThisActionAndCallbackId(string p0, string p1)
    {
      action = p0;
      callbackId = p1;
    }
        
    [Then(@"Device\.GetDeviceInfo\(\) must be called with callback id ""(.*)""")]
    public void ThenDevice_GetDeviceInfoMustBeCalledWithCallbackId(int p0)
    {
      Device.VerifyAllExpectations();
    }

    [Then( @"Success callback is called with the correct result" )]
    public void ThenSuccessCallbackIsCalledWithTheCorrectResult()
    {
      cordovaCallback.VerifyAllExpectations();
    }

  }
}
