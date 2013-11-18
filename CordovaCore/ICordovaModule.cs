using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaCore
{
  public interface ICordovaModule
  {
    string Execute(string callbackId, ICordovaCallBack cordovaCallback, string action, string args, object result);
    void Initialize();
  }
}
