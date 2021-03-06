﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaCore;
using CordovaCore.Modules;
using CordovaCore.Windows7;
using CordovaCore.Wpf;
using ReusableToolkits.Implementations;
using ReusableToolkits.Implementations.JCS;
using ReusableToolkits.Interfaces;
using StructureMap;

namespace CordovaWpfApplicationComposer
{
    public class ApplicationComposer : IApplicationComposer
    {
      #region IApplicationComposer Members

      public void ComposeApplication()
      {
        ObjectFactory.Configure( x => x.For<IMessageBox>().Use<WpfMessageBox>() );
        ObjectFactory.Configure( x => x.For<IDynamicDialog>().Use<WpfCustomDialog>() );
        ObjectFactory.Configure( x => x.For<IHtmlInteropClass>().Singleton().Use<HtmlInteropClass>() );
        //For now set the web browser controller as singleton so that all callback can be routed correctly
        ObjectFactory.Configure( x => x.For<IWebBrowserController>().Singleton().Use<WpfWebBrowserController>() );
        
        
        ObjectFactory.Configure( x => x.For<IOSVersionInfo>().Use<OSVersionInfo>() );
        ObjectFactory.Configure( x => x.For<ISettings>().Singleton().Use<Settings>() );
        ObjectFactory.Configure( x => x.For<IApplicationState>().Singleton().Use<ApplicationState>() );
        ObjectFactory.Configure( x => x.For<IMainWindow>().Singleton().Use<MainWindow>() );
        ObjectFactory.Configure( x => x.For<IApplication>().Use<CordovaAppWindows7App>()
          .EnrichWith(target => new ApplicationWithExceptionHandlers(new ApplicationWithCultureSetToUS(target), ObjectFactory.GetInstance<ISystemLog>())) );

        ObjectFactory.Configure(x => x.For<ICordovaCallBack>().Use((ICordovaCallBack)ObjectFactory.GetInstance<IWebBrowserController>()));
        RegisterCordovaModules();
      }

      private static void RegisterCordovaModules()
      {
        ObjectFactory.Configure( x => x.For<ICordovaModule>().Singleton().Use<Device>().Named( ModuleNames.Device ) );
        ObjectFactory.Configure( x => x.For<ICordovaModule>().Singleton().Use<Network>().Named( ModuleNames.Network ) );
        ObjectFactory.Configure( x => x.For<ICordovaModule>().Singleton().Use<Notification>().Named( ModuleNames.Notification ) );
        
        CordovaModules.Map = new Dictionary<string, ICordovaModule>();
        CordovaModules.Map.Add( ModuleNames.Device, ObjectFactory.GetNamedInstance<ICordovaModule>( ModuleNames.Device ) );
        CordovaModules.Map.Add( ModuleNames.Network, ObjectFactory.GetNamedInstance<ICordovaModule>( ModuleNames.Network ) );
        CordovaModules.Map.Add( ModuleNames.Notification, ObjectFactory.GetNamedInstance<ICordovaModule>( ModuleNames.Notification ) );

        CordovaModules.CordovaModulesByInitializationOrder = new List<ICordovaModule>();
        CordovaModules.CordovaModulesByInitializationOrder.Add( CordovaModules.Map[ ModuleNames.Device ] );
        CordovaModules.CordovaModulesByInitializationOrder.Add( CordovaModules.Map[ ModuleNames.Network ] );
        CordovaModules.CordovaModulesByInitializationOrder.Add( CordovaModules.Map[ ModuleNames.Notification ] );
      }

      #endregion
    }
}
