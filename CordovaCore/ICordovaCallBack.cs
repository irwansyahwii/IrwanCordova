using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaCore
{
  public enum CallbackStatuses
  {
    NoResult = 0
    , Ok = 1
    , ClassNotFoundException = 2
    , IllegalAccessException = 3
    , InstantiationException = 4
    , MalformedUrlException = 5
    , IoException = 6
    , InvalidAction = 7
    , JsonException = 8
    , GenericError = 9   
  }

  public interface ICordovaCallBack
  {
    void SuccessCallback( string callbackId, bool isKeepCallback, string message );
    void ErrorCallback( string callbackId, bool isKeepCallback, CallbackStatuses callbackStatus, string message );
  }
}
