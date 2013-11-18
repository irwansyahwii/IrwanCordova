using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ReusableToolkits.Interfaces;

namespace CordovaCore.Wpf
{
  public class WpfWebBrowserController : IWebBrowserController, ICordovaCallBack
  {
    protected WebBrowser WebBrowserControl;
    protected IHtmlInteropClass HtmlInteropClass;
    protected ExecutionStates CurrentExecutionState;
    protected ISystemLog SystemLog;

    public WpfWebBrowserController( IHtmlInteropClass htmlInteropClass
      , ISystemLog systemLog)
    {      
      CurrentExecutionState = ExecutionStates.Starting;
      HtmlInteropClass = htmlInteropClass;
      HtmlInteropClass.CordovaCallback = this;
      SystemLog = systemLog;
    }

    protected void ChangeExecutionStateTo( ExecutionStates newState )
    {
      SystemLog.LogInfo( "Changing state from {0} to {1}", CurrentExecutionState, newState );
      CurrentExecutionState = newState;
    }

    public void Initialize( object webBrowserControl )
    {
      WebBrowserControl = webBrowserControl as WebBrowser;      
      WebBrowserControl.ObjectForScripting = HtmlInteropClass;
      WebBrowserControl.Navigated += WebBrowserCotrolNavigated;
      WebBrowserControl.LoadCompleted += WebBrowserCotrolLoadCompleted;      
      WebBrowserControl.PreviewKeyDown += WebBrowserControlPreviewKeyDown;
    }

    /// <summary>
    /// TODO: Implement the appropriate handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void WebBrowserControlPreviewKeyDown( object sender, System.Windows.Input.KeyEventArgs e )
    {
      e.Handled = true;
    }

    private void WebBrowserCotrolLoadCompleted( object sender, System.Windows.Navigation.NavigationEventArgs e )
    {      
      ChangeExecutionStateTo( ExecutionStates.NativeReady );
      InvokeJSRoutine( "cordova.require('cordova/channel').onNativeReady.fire();" );
      //InvokeJSRoutine( "abcd()" );
      
    }
    void WebBrowserCotrolNavigated( object sender, System.Windows.Navigation.NavigationEventArgs e )
    {
      
    }

    public void Index()
    {
      string exePath = System.IO.Path.GetDirectoryName( Assembly.GetEntryAssembly().Location ); //@"D:/!Projects/Cordova/cordova-windows/CordovaWpf/bin/Release";
      string localFile = string.Format( @"file://127.0.0.1/{0}/www/index2.html", exePath.Replace( "C:", "c$" ).Replace( "D:", "d$" ).Replace( "E:", "e$" ) );
      Uri uri = new Uri( localFile );
            
      WebBrowserControl.Navigate( uri );
      
    }


    protected object InvokeJSRoutine(string script)
    {
      object[] args = { script };
      return WebBrowserControl.InvokeScript( "eval", args );
    }

    #region IWebBrowserController Members


    public void Resume()
    {
      //if( CurrentExecutionState == ExecutionStates.Paused )
      //{
      //  ChangeExecutionStateTo( ExecutionStates.NativeReady );
      //  InvokeJSRoutine( "cordova.require('cordova/channel').onResume.fire();" );

      //}
    }

    public void Pause()
    {
      //if( CurrentExecutionState == ExecutionStates.NativeReady )
      //{
      //  ChangeExecutionStateTo( ExecutionStates.Paused );
      //  InvokeJSRoutine( "cordova.require('cordova/channel').onPause.fire();" );
        
      //}
    }

    #endregion

    #region ICordovaCallBack Members

    private string error_string_from_code(CallbackStatuses code)
    {
	    switch (code) {
		    case CallbackStatuses.NoResult: return "cordova.callbackStatus.NO_RESULT";
		    case CallbackStatuses.Ok: return "cordova.callbackStatus.OK";
		    case CallbackStatuses.ClassNotFoundException: return "cordova.callbackStatus.CLASS_NOT_FOUND_EXCEPTION";
		    case CallbackStatuses.IllegalAccessException: return "cordova.callbackStatus.ILLEGAL_ACCESS_EXCEPTION";
		    case CallbackStatuses.InstantiationException: return "cordova.callbackStatus.INSTANTIATION_EXCEPTION";
		    case CallbackStatuses.MalformedUrlException: return "cordova.callbackStatus.MALFORMED_URL_EXCEPTION";
		    case CallbackStatuses.IoException: return "cordova.callbackStatus.IO_EXCEPTION";
		    case CallbackStatuses.InvalidAction: return "cordova.callbackStatus.INVALID_ACTION";
		    case CallbackStatuses.JsonException: return "cordova.callbackStatus.JSON_EXCEPTION";
		    default: return "cordova.callbackStatus.ERROR";
	    }
    }

    public void SuccessCallback( string callbackId, bool isKeepCallback, string message )
    {
      string jsRoutine = "window.cordova.callbackSuccess('{0}',[status:{1},keepCallback:{2},message:{3}]);";

      string statusString = string.IsNullOrEmpty( message ) ? error_string_from_code( CallbackStatuses.NoResult ) : error_string_from_code( CallbackStatuses.Ok );

      string result = string.Format( jsRoutine, callbackId, statusString, isKeepCallback ? "true" : "false", message );
      result = result.Replace( "[", "{" ).Replace( "]", "}" );

      InvokeJSRoutine( result );
    }

    public void ErrorCallback( string callbackId, bool isKeepCallback, CallbackStatuses callbackStatus, string message )
    {

      string jsRoutine = "window.cordova.callbackError('{0}',[status:{1},keepCallback:{2},message:{3}]);";

      message = "test";
      if( string.IsNullOrEmpty( message ) )
      {
        message = "\"\"";
      }
      if( !message.StartsWith( "\"" ) )
      {
        message = "\"" + message;
      }
      if( !message.EndsWith( "\"" ) )
      {
        message = message + "\"";
      }

      string statusString = string.IsNullOrEmpty( message ) ? error_string_from_code( CallbackStatuses.NoResult ) : 
        error_string_from_code( CallbackStatuses.Ok );

      string result = string.Format( jsRoutine, callbackId, statusString, isKeepCallback ? "true" : "false", message );
      result = result.Replace( "[", "{" ).Replace( "]", "}" );

      InvokeJSRoutine( result );            
    }

    #endregion
  }

  public enum ExecutionStates
  {
    Starting
    , NativeReady
    , Paused
    , Ending
  }
}
