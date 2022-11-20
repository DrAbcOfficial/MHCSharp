using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MHSharpLibrary.Util
{
    public class MHUtility
    {
        public static unsafe void SendNativeString(string str, Action<IntPtr> funcs)
        {
            IntPtr ptr = Marshal.StringToHGlobalAnsi(str);
            funcs(ptr);
            Marshal.FreeHGlobal(ptr);
        }
        /// <summary>
        /// 存在内存泄露风险，请优先使用SendNativeString
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static unsafe byte* GetNativeString(string str)
        {
            return (byte*)Marshal.StringToHGlobalAnsi(str).ToPointer();
        }
    }
}
