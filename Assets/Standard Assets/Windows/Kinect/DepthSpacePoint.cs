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
    // Windows.Kinect.DepthSpacePoint
    //
    [RootSystem.Runtime.InteropServices.StructLayout(RootSystem.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct DepthSpacePoint
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DepthSpacePoint))
            {
                return false;
            }

            return this.Equals((DepthSpacePoint)obj);
        }

        public bool Equals(DepthSpacePoint obj)
        {
            return X.Equals(obj.X) && Y.Equals(obj.Y);
        }

        public static bool operator ==(DepthSpacePoint a, DepthSpacePoint b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(DepthSpacePoint a, DepthSpacePoint b)
        {
            return !(a.Equals(b));
        }
    }

}
