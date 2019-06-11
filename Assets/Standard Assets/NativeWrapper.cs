/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace Helper
{
    public static class NativeWrapper
    {
        public static System.IntPtr GetNativePtr(Object obj)
        {
        	if(obj == null)
        	{
        		return System.IntPtr.Zero;
        	}

            var nativeWrapperIface = obj as INativeWrapper;
            if(nativeWrapperIface != null)
            {
                return nativeWrapperIface.nativePtr;
            }
            else
            {
                throw new ArgumentException("Object must wrap native type");
            }
        }
    }
}
