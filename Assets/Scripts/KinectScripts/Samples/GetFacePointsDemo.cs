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
using System.Collections.Generic;
using Microsoft.Kinect.Face;


public class GetFacePointsDemo : MonoBehaviour 
{
	private KinectManager manager = null;
	private Kinect2Interface k2interface = null;

	private Dictionary<FacePointType, Point> facePoints;


	// returns the face point coordinates or Vector2.zero if not found
	public Vector2 GetFacePoint(FacePointType pointType)
	{
		if(facePoints != null && facePoints.ContainsKey(pointType))
		{
			Point msPoint = facePoints[pointType];
			return new Vector2(msPoint.X, msPoint.Y);
		}

		return Vector3.zero;
	}

	void Update () 
	{
		// get reference to the Kinect2Interface
		if(k2interface == null)
		{
			manager = KinectManager.Instance;
			
			if(manager && manager.IsInitialized())
			{
				KinectInterop.SensorData sensorData = manager.GetSensorData();
				
				if(sensorData != null && sensorData.sensorInterface != null)
				{
					k2interface = (Kinect2Interface)sensorData.sensorInterface;
				}
			}
		}

		// get the face points
		if(k2interface != null && k2interface.faceFrameResults != null)
		{
			if(manager != null && manager.IsUserDetected())
			{
				ulong userId = (ulong)manager.GetPrimaryUserID();
				
				for(int i = 0; i < k2interface.faceFrameResults.Length; i++)
				{
					if(k2interface.faceFrameResults[i] != null && k2interface.faceFrameResults[i].TrackingId == userId)
					{
						facePoints = k2interface.faceFrameResults[i].FacePointsInColorSpace;
						break;
					}
				}
			}
		}

	}


}
