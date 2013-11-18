using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaCore;
using CordovaCore.Wpf;
using NUnit.Framework;
using ReusableToolkits.Interfaces;
using Rhino.Mocks;
using TechTalk.SpecFlow;
using IrwanUnitTestFramework;
using System.Windows.Input;
using System.Windows;

namespace CordovaTests.Tests.Modules
{
  [Binding]
  public class WpfWebBrowserControllerTests
  {
    protected IHtmlInteropClass HtmlInteropClass;
    protected WpfWebBrowserController WpfWebBrowserController;

    [Given( @"an instance of IHtmlInteropClass" )]
    public void GivenAnInstanceOfIHtmlInteropClass()
    {
      HtmlInteropClass = MockRepository.GenerateMock<IHtmlInteropClass>();
      HtmlInteropClass.Stub( x => x.CordovaCallback ).PropertyBehavior();
    }

    [When( @"WpfWebBrowserController is instantiated" )]
    public void WhenWpfWebBrowserControllerIsInstantiated()
    {
      WpfWebBrowserController = new WpfWebBrowserController( HtmlInteropClass, MockRepository.GenerateMock<ISystemLog>() );
    }

    [Then( @"IHtmlInteropClass's CordovaCallback propery must be set with the WpfWebBrowserController instance" )]
    public void ThenIHtmlInteropClassSCordovaCallbackProperyMustBeSetWithTheWpfWebBrowserControllerInstance()
    {
      Assert.IsNotNull( HtmlInteropClass.CordovaCallback );
      Assert.AreEqual( WpfWebBrowserController, HtmlInteropClass.CordovaCallback );
    }

  }
}
