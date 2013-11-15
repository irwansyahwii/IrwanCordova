using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReusableToolkits.Implementations.SystemLog;
using ReusableToolkits.Interfaces;
using StructureMap;

namespace CordovaCore
{
  public class CompositionRoot
  {
    public static void ComposeApplication()
    {         
      ObjectFactory.Configure( x => x.For<ISystemLog>().Use<LogToConsole>().EnrichWith(target => new LogToDebugWindow(target)));

      ISystemLog systemLog = ObjectFactory.GetInstance<ISystemLog>();

      try{
        ObjectFactory.Configure( x => x.Scan( scanner =>
        {        
          systemLog.LogInfo("Scanning for assembly in application base dir with name containings 'ApplicationComposer'");
          scanner.AssembliesFromApplicationBaseDirectory(asm => asm.FullName.Contains("ApplicationComposer"));
                
          scanner.AddAllTypesOf<IApplicationComposer>();
          scanner.WithDefaultConventions();        
        } ) );

        IApplicationComposer composer = ObjectFactory.GetInstance<IApplicationComposer>();
        composer.ComposeApplication();
      }
      catch(Exception ex){
        systemLog.LogException(ex, "CompositionRoot.ComposeApplication()");
        throw ex;
      }
    }
  }
}
