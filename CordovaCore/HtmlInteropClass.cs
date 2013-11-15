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
    public HtmlInteropClass( ISystemLog systemLog )
    {
      SystemLog = systemLog;
    }

    public void CordovaExec(string callbackId, string service, string action, string args)
    {
      try
      {
        if( CordovaModules.Map.ContainsKey( service ) )
        {
          CordovaModules.Map[ service ].Execute( callbackId, action, args, string.Empty );
        }
        else
        {
          //throw service not found
        }

      }
      catch( Exception ex)
      {
        SystemLog.LogException( ex, "HtmlInteropClass.CordovaExec" );
        throw ex;
      }
    }

    public void MyMethod(string SomeValue)
    {

      //((MainWindow)Application.Current.MainWindow).SomeTextBox.Text = SomeValue;
    }
  }
}
