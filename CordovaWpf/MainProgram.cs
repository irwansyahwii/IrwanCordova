using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CordovaCore;
using ReusableToolkits.Interfaces;
using StructureMap;

namespace CordovaWpf
{
  public class MainProgram
  {
    [STAThread]
    public static void Main()
    {
      ISystemLog systemLog = null;      
      try
      {
        CompositionRoot.ComposeApplication();

        systemLog = ObjectFactory.GetInstance<ISystemLog>();

        IApplication app = ObjectFactory.GetInstance<IApplication>();
        app.Initialize();
        app.Run();
      }
      catch( Exception ex )
      {
        MessageBox.Show( ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error );

        if( systemLog != null )
        {
          systemLog.LogException( ex, "MainProgram.Main()" );
        }        
      }
    }
  }
}
