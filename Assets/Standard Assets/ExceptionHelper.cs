/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Runtime.InteropServices;

namespace Helper
{
    public static class ExceptionHelper
    {
        private const int E_NOTIMPL = unchecked((int)0x80004001);
        private const int E_OUTOFMEMORY = unchecked((int)0x8007000E);
        private const int E_INVALIDARG = unchecked((int)0x80070057);
        private const int E_POINTER = unchecked((int) 0x80004003);
        private const int E_PENDING = unchecked((int)0x8000000A);
        private const int E_FAIL = unchecked((int)0x80004005);

        public static void CheckLastError()
        {
            int hr = Marshal.GetLastWin32Error();

            if ((hr == E_PENDING) || (hr == E_FAIL))
            {
                // Ignore E_PENDING/E_FAIL - We use this to indicate no pending or missed frames
                return;
            }

            if (hr < 0)
            {
                Exception exception = Marshal.GetExceptionForHR(hr);
                string message = string.Format("This API has returned an exception from an HRESULT: 0x{0:X}", hr);

                switch (hr)
                {
                    case E_NOTIMPL:
                        throw new NotImplementedException(message, exception);

                    case E_OUTOFMEMORY:
                        throw new OutOfMemoryException(message, exception);

                    case E_INVALIDARG:
                        throw new ArgumentException(message, exception);

                    case E_POINTER:
                        throw new ArgumentNullException(message, exception);

                    default:
                        throw new InvalidOperationException(message, exception);
                }
            }
        }
    }
}
