using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MHSharpLibrary.Util
{
    public class Utility
    {
        public static unsafe byte* GetNativeString(string str)
        {
            return (byte*)Marshal.StringToHGlobalAnsi(str).ToPointer();
        }
    }
}
