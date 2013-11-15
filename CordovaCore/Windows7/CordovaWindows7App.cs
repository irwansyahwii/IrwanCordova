using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CordovaCore.Modules;
using Microsoft.Win32;
using ReusableToolkits.Interfaces;

namespace CordovaCore.Windows7
{
  public class CordovaAppWindows7App : IApplication
  {
    const string IE_GPU_REG_KEY = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_GPU_RENDERING";		// Registry key enabling GPU acceleration
    const string IE_COMPAT_REG_KEY = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";	// Registry key controlling browser version emulation

    protected IOSVersionInfo OSVersionInfo;
    protected ISettings Settings;
    protected IApplicationState ApplicationState;
    protected IMainWindow MainWindow;    
    
    public CordovaAppWindows7App( IOSVersionInfo osVersionInfo
      , ISettings settings
      , IApplicationState applicationState
      , IMainWindow mainWindow
      )
    {
      OSVersionInfo = osVersionInfo;
      Settings = settings;
      ApplicationState = applicationState;
      MainWindow = mainWindow;
    }

    protected void InitBrowserEmulation()
    {
      string appName = "";

      //appName = Path.GetFileName( Assembly.GetEntryAssembly().Location );
      appName = System.IO.Path.GetFileName( Process.GetCurrentProcess().MainModule.FileName );

      const string IE_EMULATION = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

      using( var fbeKey = Registry.CurrentUser.OpenSubKey( IE_EMULATION, RegistryKeyPermissionCheck.ReadWriteSubTree ) )
      {
        fbeKey.SetValue( appName, 9999, RegistryValueKind.DWord );          
      }
    }

    protected void SetMainThreadName()
    {
      Thread.CurrentThread.Name = "Primary Thread";
    }

    #region IApplication Members
    public void Initialize()
    {
      ApplicationState.FunctionInitialize = () =>
      {
        SetMainThreadName();

        CheckWindowsOSVersion();
        CheckIEVersion();
        SaveApplicationBaseDirectory();
        InitIEGPUAcceleration();
        InitBrowserEmulation();
        RegisterCordovaModules();

        ApplicationState.EventInitialized();
      };

      ApplicationState.FunctionRun = () =>
      {
        Application app = new Application();
        app.Run( MainWindow.GetWindow() );
      };
      ApplicationState.FunctionShuttingDown = () => {
      };
      ApplicationState.FunctionShowConfirmQuit = () => {
      };
      ApplicationState.FunctionDisplayAndLogException = (ex) =>
      {
      };     
      ApplicationState.FunctionExitApp = () =>
      {
        RunInUIThread( UIThreadExitApp );
      };

      
    }

    private void RegisterCordovaModules()
    {
      if( CordovaModules.CordovaModulesByInitializationOrder != null )
      {
        foreach( var module in CordovaModules.CordovaModulesByInitializationOrder )
        {
          module.Initialize();
        }
      }
    }

    protected void RunInUIThread( Action action )
    {
      if( Application.Current != null )
      {
        if( !Application.Current.Dispatcher.HasShutdownStarted )
        {
          try
          {
            Application.Current.Dispatcher.Invoke( action );
          }
          catch( Exception ex )
          {
            ApplicationState.EventException( ex );
          }

        }
      }
    }

    protected void UIThreadExitApp()
    {
      if( Application.Current != null )
      {
        Application.Current.Shutdown( 0 );
      }
    }

    private void InitIEGPUAcceleration()
    {
      string appName = "";

      appName = Path.GetFileName( Assembly.GetEntryAssembly().Location );
      
      using( var fbeKey = Registry.CurrentUser.OpenSubKey( IE_GPU_REG_KEY, RegistryKeyPermissionCheck.ReadWriteSubTree ) )
      {
        fbeKey.SetValue( appName, 1, RegistryValueKind.DWord );
      }
    }

    private void SaveApplicationBaseDirectory()
    {
      Settings.ApplicationBaseDirectory = GetCurrentBaseDirectory();
    }

    private string GetCurrentBaseDirectory()
    {
      return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }

    private void CheckIEVersion()
    {
      string appName = "";

      appName = Path.GetFileName( Assembly.GetEntryAssembly().Location );        

      var ieVersion = Registry.LocalMachine.OpenSubKey( @"Software\Microsoft\Internet Explorer" ).GetValue( "Version" );

      float versionValue = 0;

      if( ieVersion != null )
      {
        string stringVersion = ieVersion.ToString();
        if( stringVersion.Length >= 3 )
        {
          stringVersion = stringVersion.Substring( 0, 3 );
          float.TryParse( stringVersion, out versionValue );
        }        
      }
      
      if( versionValue < 9 )
      {
        throw new ApplicationException( "This program requires Internet Explorer 9 or newer" );
      }
    }

    private void CheckWindowsOSVersion()
    {      
	    if (OSVersionInfo.MajorVersion < 6 || (OSVersionInfo.MajorVersion == 6 && OSVersionInfo.MinorVersion < 1))
	    {
        throw new ApplicationException("This program requires Windows 7 or newer.");
	    }

    }

    public void Run()
    {
      ApplicationState.EventInitializeStart();      
    }

    public void Stop()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
