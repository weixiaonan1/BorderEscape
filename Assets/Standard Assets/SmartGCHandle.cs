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
    public class SmartGCHandle : IDisposable
    {
        private GCHandle handle;
        public SmartGCHandle(GCHandle handle)
        {
            this.handle = handle;
        }

        ~SmartGCHandle()
        {
            Dispose(false);
        }

        public System.IntPtr AddrOfPinnedObject()
        {
            return handle.AddrOfPinnedObject();
        }

        public virtual void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.handle.Free();
        }

        public static implicit operator GCHandle(SmartGCHandle other)
        {

            return other.handle;
        }
    }
}
