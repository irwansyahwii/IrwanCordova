using System;
namespace CordovaCore
{
  public interface IHtmlInteropClass
  {
    ICordovaCallBack CordovaCallback { get; set; }
    void CordovaExec( string callbackId, string service, string action, string args );
  }
}
