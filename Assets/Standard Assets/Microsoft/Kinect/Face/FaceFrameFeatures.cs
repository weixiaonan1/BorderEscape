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
    // Microsoft.Kinect.Face.FaceFrameFeatures
    //
    [RootSystem.Flags]
    public enum FaceFrameFeatures : uint
    {
        None                                     =0,
        BoundingBoxInInfraredSpace               =1,
        PointsInInfraredSpace                    =2,
        BoundingBoxInColorSpace                  =4,
        PointsInColorSpace                       =8,
        RotationOrientation                      =16,
        Happy                                    =32,
        RightEyeClosed                           =64,
        LeftEyeClosed                            =128,
        MouthOpen                                =256,
        MouthMoved                               =512,
        LookingAway                              =1024,
        Glasses                                  =2048,
        FaceEngagement                           =4096,
    }

}
