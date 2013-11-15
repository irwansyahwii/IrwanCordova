using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CordovaCore;
using ReusableToolkits.Interfaces;

namespace CordovaCore.Wpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, IMainWindow
  {
    protected IWebBrowserController WebBrowserController;
    public MainWindow(IWebBrowserController webBrowserController)
    {
      InitializeComponent();
      WebBrowserController = webBrowserController;
      WebBrowserController.Initialize(WebView);

      this.Activated += MainWindow_Activated;
      this.Deactivated += MainWindow_Deactivated;
    }

    void MainWindow_Deactivated( object sender, EventArgs e )
    {
      WebBrowserController.Resume();
    }

    void MainWindow_Activated( object sender, EventArgs e )
    {
      WebBrowserController.Pause();
    }

    //private void WebView_Loaded( object sender, RoutedEventArgs e )
    //{
    //  string exePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); //@"D:/!Projects/Cordova/cordova-windows/CordovaWpf/bin/Release";
    //  string localFile = string.Format(@"file://127.0.0.1/{0}/www/index.html", exePath.Replace("C:", "c$").Replace("D:", "d$").Replace("E:", "e$"));
    //  Uri uri = new Uri(localFile);

    //  WebView.ObjectForScripting = new HtmlInteropClass();
    //  WebView.Navigated += WebView_Navigated;
    //  WebView.Navigate( uri );
    //  WebView.LoadCompleted += WebView_LoadCompleted;
    //}

    //void WebView_LoadCompleted( object sender, NavigationEventArgs e )
    //{
    //  //WebView.InvokeScript( "alert", new string[] {"Load completed" } );
    //  WebView.InvokeScript( "abcd" );
    //  //WebView.InvokeScript( "eval", new string[] {"abcd()"} );

    //  if( e.Uri.PathAndQuery.EndsWith( "index.html" ) )
    //  {
        
        
    //  }
      
    //}

    //void WebView_Navigated( object sender, NavigationEventArgs e )
    //{
    //  Debug.Print( "adsad" );
    //  Debug.Print( e.Uri.PathAndQuery );
    //  if( e.Uri.PathAndQuery.EndsWith( "index.html" ) )
    //  {
        
    //  }
    //}

    #region IMainWindow Members

    public Window GetWindow()
    {
      return this;
    }

    #endregion

    private void Window_Loaded( object sender, RoutedEventArgs e )
    {
      WebBrowserController.Index();
    }
  }
}
