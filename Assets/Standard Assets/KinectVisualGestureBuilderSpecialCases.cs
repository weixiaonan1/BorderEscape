/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using RootSystem = System;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Kinect.VisualGestureBuilder
{
    public sealed partial class VisualGestureBuilderDatabase
    {
        [RootSystem.Runtime.InteropServices.DllImport(
            "KinectVisualGestureBuilderUnityAddin",
            EntryPoint = "Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderDatabase_ctor",
            CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderDatabase_ctor([MarshalAs(UnmanagedType.LPWStr)]string path);
        public static VisualGestureBuilderDatabase Create(string path)
        {
            RootSystem.IntPtr objectPointer = Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderDatabase_ctor(path);
            Helper.ExceptionHelper.CheckLastError();
            if (objectPointer == RootSystem.IntPtr.Zero)
            {
                return null;
            }

            return Helper.NativeObjectCache.CreateOrGetObject<Microsoft.Kinect.VisualGestureBuilder.VisualGestureBuilderDatabase>(
                objectPointer, n => new Microsoft.Kinect.VisualGestureBuilder.VisualGestureBuilderDatabase(n));
        }
    }

    public sealed partial class VisualGestureBuilderFrameSource
    {
        [RootSystem.Runtime.InteropServices.DllImport(
            "KinectVisualGestureBuilderUnityAddin",
            EntryPoint = "Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderFrameSource_ctor",
            CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern RootSystem.IntPtr Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderFrameSource_ctor(RootSystem.IntPtr sensorPtr, ulong initialTrackingId);
        public static VisualGestureBuilderFrameSource Create(Windows.Kinect.KinectSensor sensor, ulong initialTrackingId)
        {
            RootSystem.IntPtr objectPointer = Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderFrameSource_ctor(Helper.NativeWrapper.GetNativePtr(sensor), initialTrackingId);
            Helper.ExceptionHelper.CheckLastError();
            if (objectPointer == RootSystem.IntPtr.Zero)
            {
                return null;
            }

            return Helper.NativeObjectCache.CreateOrGetObject<Microsoft.Kinect.VisualGestureBuilder.VisualGestureBuilderFrameSource>(
                objectPointer, n => new Microsoft.Kinect.VisualGestureBuilder.VisualGestureBuilderFrameSource(n));
        }

        [RootSystem.Runtime.InteropServices.DllImport(
            "KinectVisualGestureBuilderUnityAddin",
            EntryPoint = "Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderFrameSource_GetIsEnabled",
            CallingConvention = RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern bool Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderFrameSource_GetIsEnabled(RootSystem.IntPtr pNative, RootSystem.IntPtr gesturePtr);
        public bool GetIsEnabled(Microsoft.Kinect.VisualGestureBuilder.Gesture gesture)
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                throw new RootSystem.ObjectDisposedException("VisualGestureBuilderFrameSource");
            }

            var result = Microsoft_Kinect_VisualGestureBuilder_VisualGestureBuilderFrameSource_GetIsEnabled(_pNative, Helper.NativeWrapper.GetNativePtr(gesture));
            Helper.ExceptionHelper.CheckLastError();
            return result;
        }
    }
}
