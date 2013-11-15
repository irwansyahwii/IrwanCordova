using System;
namespace CordovaCore
{
  public interface IWebBrowserController
  {
    void Index();
    void Initialize(object webBrowserControl);

    void Resume();

    void Pause();
  }
}
