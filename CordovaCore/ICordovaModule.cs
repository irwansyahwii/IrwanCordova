using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaCore
{
  public interface ICordovaModule
  {
    void Execute(string callbackId, string action, string args, object result);
    void Initialize();
  }
}
