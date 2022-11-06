using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFramework.Util;

public class OpenGL
{
    static GL? _gl = null;
    static IntPtr GlMoudle = default;

    public static GL gl
    {
        get
        {
            if (_gl == null)
            {
                GlMoudle = Native.LoadLibrary("opengl32.dll");
                _gl = GL.GetApi(new Func<string, nint>(GetProcAddress));
            }
            return _gl;
        }
    }

    static unsafe nint GetProcAddress(string name)
    {
        var fun = Native.GetProcAddress(GlMoudle, name);
        return fun;
    }
}
