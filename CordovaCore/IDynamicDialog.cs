using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaCore
{
  public interface IDynamicDialog
  {
    void Initialize();
    void AddButton( string caption );
    string ButtonChosen { get; set; }
    string ShowDialog();
  }
}
