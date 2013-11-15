using System;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace DotNetCPPBridge
{    
    public static class Exports
    {

      [DllExport]
      public static int Hello()
      {
        return 1;
      }

      [DllExport("CallInstanceMethod", CallingConvention= CallingConvention.Cdecl)]
      [return: MarshalAs( UnmanagedType.BStr )]
      public static string CallInstanceMethod( [MarshalAs( UnmanagedType.BStr )] string className,
        [MarshalAs( UnmanagedType.BStr )]
        string methodName,
        [MarshalAs( UnmanagedType.BStr )]
        string args )
      {
        return string.Empty;
      }      
    }
}
