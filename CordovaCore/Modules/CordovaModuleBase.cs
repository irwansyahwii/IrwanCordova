using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaCore.Modules
{
  public class CordovaModuleBase 
  {
    protected string ThrowMemberNotFound(string action)
    {
      throw new ApplicationException( "Member not found: " + action );
    }
  }
}
