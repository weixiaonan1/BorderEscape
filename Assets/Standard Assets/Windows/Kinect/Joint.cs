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
    // Windows.Kinect.Joint
    //
    [RootSystem.Runtime.InteropServices.StructLayout(RootSystem.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Joint
    {
        public Windows.Kinect.JointType JointType { get; set; }
        public Windows.Kinect.CameraSpacePoint Position { get; set; }
        public Windows.Kinect.TrackingState TrackingState { get; set; }

        public override int GetHashCode()
        {
            return JointType.GetHashCode() ^ Position.GetHashCode() ^ TrackingState.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Joint))
            {
                return false;
            }

            return this.Equals((Joint)obj);
        }

        public bool Equals(Joint obj)
        {
            return JointType.Equals(obj.JointType) && Position.Equals(obj.Position) && TrackingState.Equals(obj.TrackingState);
        }

        public static bool operator ==(Joint a, Joint b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Joint a, Joint b)
        {
            return !(a.Equals(b));
        }
    }

}
