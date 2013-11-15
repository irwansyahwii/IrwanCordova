using System;
namespace CordovaCore
{
  public interface IHtmlInteropClass
  {
    void CordovaExec( string callbackId, string service, string action, string args );
  }
}
