using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using ReusableToolkits.Interfaces;
using Technewlogic.Samples.WpfModalDialog;

namespace CordovaCore.Modules
{
  public class Notification : CordovaModuleBase, ICordovaModule
  {
    protected IMessageBox MessageBox;
    protected Func<IDynamicDialog> DynamicDialogFactory;
    public Notification( IMessageBox messageBox 
      , Func<IDynamicDialog> dynamicDialogFactory)
    {
      MessageBox = messageBox;
      DynamicDialogFactory = dynamicDialogFactory;
    }

    #region ICordovaModule Members

    public string Execute( string callbackId, ICordovaCallBack cordovaCallback, string action, string args, object result )
    {      
      if( action.Equals( "alert" ) || action.Equals( "confirm" ) )
      {
        return showDialog( callbackId, cordovaCallback, args );
      }
      if(action.Equals("vibrate"))
      {
        return vibrate( callbackId, cordovaCallback, args );
      }
      if( action.Equals( "beep" ) )
      {
        return beep( callbackId, cordovaCallback, args );
      }
      return ThrowMemberNotFound( action );      
    }

    protected string beep( string callbackId, ICordovaCallBack cordovaCallback, string args )
    {
      JavaScriptSerializer serializer = new JavaScriptSerializer();
      int[] array = (int[])serializer.Deserialize( args, typeof(int[]));
      if( array.Length > 0 )
      {
        for( int i = 0; i < array[0]; i++ )
        {
          System.Media.SystemSounds.Beep.Play();
        }
      }

      return string.Empty;
    }

    protected string vibrate( string callbackId, ICordovaCallBack cordovaCallback, string args )
    {
      throw new NotImplementedException();
    }

    protected string showDialog( string callbackId, ICordovaCallBack cordovaCallback, string args )
    {
      // args should be like "["message","title","button1,button2"]"
      JavaScriptSerializer serializer = new JavaScriptSerializer();
      string[] array = (string[]) serializer.Deserialize( args, typeof( string[] ) );

      //var window = new Window();
      //var stackPanel = new WpfDynamicDialog
      //window.Title = array[ 1 ];

      IDynamicDialog dlg = DynamicDialogFactory();
      dlg.Initialize();
      var buttons = array[ 2 ].Split( ",".ToCharArray() );
      foreach( var item in buttons )
      {
        dlg.AddButton(item);
        //stackPanel.Children.Add( new Button { Content = item } );
      }
            
      //window.Content = stackPanel;
      //window.ShowDialog();
      string dlgResult = dlg.ShowDialog();

      cordovaCallback.SuccessCallback( callbackId, false, dlgResult );

      return "";        
    }

    public void Initialize()
    {
      
    }

    #endregion
  }
}
