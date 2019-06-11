/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

public class FollowUserRotation : MonoBehaviour 
{
	void Update () 
	{
		KinectManager manager = KinectManager.Instance;

		if(manager && manager.IsInitialized())
		{
			if(manager.IsUserDetected())
			{
				long userId = manager.GetPrimaryUserID();

				if(manager.IsJointTracked(userId, (int)KinectInterop.JointType.ShoulderLeft) &&
				   manager.IsJointTracked(userId, (int)KinectInterop.JointType.ShoulderRight))
				{
					Vector3 posLeftShoulder = manager.GetJointPosition(userId, (int)KinectInterop.JointType.ShoulderLeft);
					Vector3 posRightShoulder = manager.GetJointPosition(userId, (int)KinectInterop.JointType.ShoulderRight);

					posLeftShoulder.z = -posLeftShoulder.z;
					posRightShoulder.z = -posRightShoulder.z;

					Vector3 dirLeftRight = posRightShoulder - posLeftShoulder;
					dirLeftRight -= Vector3.Project(dirLeftRight, Vector3.up);

					Quaternion rotationShoulders = Quaternion.FromToRotation(Vector3.right, dirLeftRight);

					transform.rotation = rotationShoulders;
				}
			}
		}
	}
}
