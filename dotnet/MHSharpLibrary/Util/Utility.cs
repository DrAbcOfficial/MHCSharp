using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
namespace MHSharpLibrary.Util
{
    public class MHUtility
    {
        /// <summary>
        /// 需要在游戏中长时间存在时使用这个，比如添加Command到控制台
        /// </summary>
        /// <param name="str"></param>
        /// <param name="funcs"></param>
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
