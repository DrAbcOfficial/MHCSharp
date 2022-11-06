using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MHFramework.SDK;

public class CLEngineFucs
{
    public static CLEngineFucsStruct FunSet;
        
    public static unsafe void ConsolePrint(string message)
    {
        FunSet.ConPrintf((byte*)Marshal.StringToHGlobalAnsi(message).ToPointer());
    }
    public static unsafe void ConsolePrintLine(string message)
    {
        FunSet.ConPrintf((byte*)Marshal.StringToHGlobalAnsi(message + "\n").ToPointer());
    }
}
