using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaCore.Modules;
using ReusableToolkits.Interfaces;

namespace CordovaCore
{
  [System.Runtime.InteropServices.ComVisibleAttribute(true)]
  public class HtmlInteropClass : CordovaCore.IHtmlInteropClass
  {
    protected ISystemLog SystemLog;
    public ICordovaCallBack CordovaCallback { get; set; }
    
    public HtmlInteropClass( ISystemLog systemLog)
    {
      SystemLog = systemLog;      
    }

    public void CordovaExec(string callbackId, string service, string action, string args)
    {
      SystemLog.LogInfo( "Receiving command callbackId:{0}, service:{1}, action:{2}, args:{3}"
        , callbackId
        , service
        , action
        , args );

      try
      {
        if( CordovaModules.Map.ContainsKey( service ) )
        {
          CordovaModules.Map[ service ].Execute( callbackId, CordovaCallback, action, args, string.Empty );
        }
        else
        {
          //throw service not found
          CordovaCallback.ErrorCallback( callbackId, false, CallbackStatuses.ClassNotFoundException, string.Empty );
        }

      }
      catch( Exception ex)
      {        
        SystemLog.LogException( ex, "HtmlInteropClass.CordovaExec" );
        CordovaCallback.ErrorCallback( callbackId, false, CallbackStatuses.GenericError, ex.ToString() );
        //throw ex;
      }
    }

    public void MyMethod(string SomeValue)
    {

      //((MainWindow)Application.Current.MainWindow).SomeTextBox.Text = SomeValue;
    }
  }
}
