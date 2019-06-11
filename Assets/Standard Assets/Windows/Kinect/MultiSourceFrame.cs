/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using RootSystem = System;
using System.Linq;
using System.Collections.Generic;
namespace Windows.Kinect
{
    //
    // Windows.Kinect.MultiSourceFrame
    //
    public sealed partial class MultiSourceFrame : Helper.INativeWrapper

    {
        internal RootSystem.IntPtr _pNative;
        RootSystem.IntPtr Helper.INativeWrapper.nativePtr { get { return _pNative; } }

        // Constructors and Finalizers
        internal MultiSourceFrame(RootSystem.IntPtr pNative)
        {
            _pNative = pNative;
            Windows_Kinect_MultiSourceFrame_AddRefObject(ref _pNative);
        }

        ~MultiSourceFrame()
        {
            Dispose(false);
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern void Windows_Kinect_MultiSourceFrame_ReleaseObject(ref RootSystem.IntPtr pNative);
        [RootSystem.Runtime.InteropServices.DllImport("KinectUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern void Windows_Kinect_MultiSourceFrame_AddRefObject(ref RootSystem.IntPtr pNative);
        private void Dispose(bool disposing)
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                return;
            }

            __EventCleanup();

            Helper.NativeObjectCache.RemoveObject<MultiSourceFrame>(_pNative);
                Windows_Kinect_MultiSourceFrame_ReleaseObject(ref _pNative);

            _pNative = RootSystem.IntPtr.Zero;
        }


        // Public Properties
        [RootSystem.Runtime.InteropServices.DllImport("KinectUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern RootSystem.IntPtr Windows_Kinect_MultiSourceFrame_get_BodyFrameReference(RootSystem.IntPtr pNative);
        public  Windows.Kinect.BodyFrameReference BodyFrameReference
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("MultiSourceFrame");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_MultiSourceFrame_get_BodyFrameReference(_pNative);
                Helper.ExceptionHelper.CheckLastError();
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.BodyFrameReference>(objectPointer, n => new Windows.Kinect.BodyFrameReference(n));
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern RootSystem.IntPtr Windows_Kinect_MultiSourceFrame_get_BodyIndexFrameReference(RootSystem.IntPtr pNative);
        public  Windows.Kinect.BodyIndexFrameReference BodyIndexFrameReference
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("MultiSourceFrame");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_MultiSourceFrame_get_BodyIndexFrameReference(_pNative);
                Helper.ExceptionHelper.CheckLastError();
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.BodyIndexFrameReference>(objectPointer, n => new Windows.Kinect.BodyIndexFrameReference(n));
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern RootSystem.IntPtr Windows_Kinect_MultiSourceFrame_get_ColorFrameReference(RootSystem.IntPtr pNative);
        public  Windows.Kinect.ColorFrameReference ColorFrameReference
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("MultiSourceFrame");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_MultiSourceFrame_get_ColorFrameReference(_pNative);
                Helper.ExceptionHelper.CheckLastError();
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.ColorFrameReference>(objectPointer, n => new Windows.Kinect.ColorFrameReference(n));
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern RootSystem.IntPtr Windows_Kinect_MultiSourceFrame_get_DepthFrameReference(RootSystem.IntPtr pNative);
        public  Windows.Kinect.DepthFrameReference DepthFrameReference
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("MultiSourceFrame");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_MultiSourceFrame_get_DepthFrameReference(_pNative);
                Helper.ExceptionHelper.CheckLastError();
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.DepthFrameReference>(objectPointer, n => new Windows.Kinect.DepthFrameReference(n));
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern RootSystem.IntPtr Windows_Kinect_MultiSourceFrame_get_InfraredFrameReference(RootSystem.IntPtr pNative);
        public  Windows.Kinect.InfraredFrameReference InfraredFrameReference
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("MultiSourceFrame");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_MultiSourceFrame_get_InfraredFrameReference(_pNative);
                Helper.ExceptionHelper.CheckLastError();
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.InfraredFrameReference>(objectPointer, n => new Windows.Kinect.InfraredFrameReference(n));
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern RootSystem.IntPtr Windows_Kinect_MultiSourceFrame_get_LongExposureInfraredFrameReference(RootSystem.IntPtr pNative);
        public  Windows.Kinect.LongExposureInfraredFrameReference LongExposureInfraredFrameReference
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("MultiSourceFrame");
                }

                RootSystem.IntPtr objectPointer = Windows_Kinect_MultiSourceFrame_get_LongExposureInfraredFrameReference(_pNative);
                Helper.ExceptionHelper.CheckLastError();
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.LongExposureInfraredFrameReference>(objectPointer, n => new Windows.Kinect.LongExposureInfraredFrameReference(n));
            }
        }


        // Public Methods
        private void __EventCleanup()
        {
        }
    }

}
