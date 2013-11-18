using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaCore
{
  public static class CordovaModules
  {
    public static Dictionary<string, ICordovaModule> Map
    {
      get;
      set;
    }

    public static List<ICordovaModule> CordovaModulesByInitializationOrder
    {
      get;
      set;
    }
  }
}
