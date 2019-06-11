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
    // Microsoft.Kinect.Face.FaceModelBuilderCollectionStatus
    //
    [RootSystem.Flags]
    public enum FaceModelBuilderCollectionStatus : uint
    {
        Complete                                 =0,
        MoreFramesNeeded                         =1,
        FrontViewFramesNeeded                    =2,
        LeftViewsNeeded                          =4,
        RightViewsNeeded                         =8,
        TiltedUpViewsNeeded                      =16,
    }

}
