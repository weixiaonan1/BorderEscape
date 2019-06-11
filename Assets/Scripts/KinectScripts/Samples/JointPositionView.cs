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
//using Windows.Kinect;


public class JointPositionView : MonoBehaviour 
{
	[Tooltip("The Kinect joint we want to track.")]
	public KinectInterop.JointType trackedJoint = KinectInterop.JointType.SpineBase;

	[Tooltip("Whether the movement is relative to transform's initial position, or is in absolute coordinates.")]
	public bool relToInitialPos = false;
	
	[Tooltip("Whether the z-movement is inverted or not.")]
	public bool invertedZMovement = false;

	[Tooltip("Transform offset to the Kinect-reported position.")]
	public Vector3 transformOffset = Vector3.zero;

	[Tooltip("Whether the displayed position is in Kinect coordinates, or in world coordinates.")]
	public bool useKinectSpace = false;

	//public bool moveTransform = true;

	[Tooltip("Smooth factor used for the joint position smoothing.")]
	public float smoothFactor = 5f;

	[Tooltip("GUI-Text to display the current joint position.")]
	public GUIText debugText;


	private Vector3 initialPosition = Vector3.zero;
	private long currentUserId = 0;
	private Vector3 initialUserOffset = Vector3.zero;

	private Vector3 vPosJoint = Vector3.zero;


	void Start()
	{
		initialPosition = transform.position;
	}
	
	void Update () 
	{
		KinectManager manager = KinectManager.Instance;
		
		if(manager && manager.IsInitialized())
		{
			int iJointIndex = (int)trackedJoint;

			if(manager.IsUserDetected())
			{
				long userId = manager.GetPrimaryUserID();
				
				if(manager.IsJointTracked(userId, iJointIndex))
				{
					if(useKinectSpace)
						vPosJoint = manager.GetJointKinectPosition(userId, iJointIndex);
					else
						vPosJoint = manager.GetJointPosition(userId, iJointIndex);

					vPosJoint.z = invertedZMovement ? -vPosJoint.z : vPosJoint.z;
					vPosJoint += transformOffset;

					if(userId != currentUserId)
					{
						currentUserId = userId;
						initialUserOffset = vPosJoint;
					}

					Vector3 vPosObject = relToInitialPos ? initialPosition + (vPosJoint - initialUserOffset) : vPosJoint;

					if(debugText)
					{
						debugText.GetComponent<GUIText>().text = string.Format("{0} - ({1:F3}, {2:F3}, {3:F3})", trackedJoint, 
						                                                       vPosObject.x, vPosObject.y, vPosObject.z);
					}

					//if(moveTransform)
					{
						if(smoothFactor != 0f)
							transform.position = Vector3.Lerp(transform.position, vPosObject, smoothFactor * Time.deltaTime);
						else
							transform.position = vPosObject;
					}
				}
				
			}
			
		}
	}
}
