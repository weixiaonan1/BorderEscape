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
namespace Microsoft.Kinect.Face
{
    //
    // Microsoft.Kinect.Face.RectI
    //
    [RootSystem.Runtime.InteropServices.StructLayout(RootSystem.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct RectI
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RectI))
            {
                return false;
            }

            return this.Equals((RectI)obj);
        }

        public bool Equals(RectI obj)
        {
            return Left.Equals(obj.Left) && Top.Equals(obj.Top) && Right.Equals(obj.Right) && Bottom.Equals(obj.Bottom);
        }

        public static bool operator ==(RectI a, RectI b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(RectI a, RectI b)
        {
            return !(a.Equals(b));
        }
    }

}
