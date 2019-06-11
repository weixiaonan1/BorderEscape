using UnityEngine;
//using Windows.Kinect;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 


/// <summary>
/// Avatar controller is the component that transfers the captured user motion to a humanoid model (avatar).
/// </summary>
[RequireComponent(typeof(Animator))]
public class AvatarController : MonoBehaviour
{	
	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;

	[Tooltip("Whether the avatar is facing the player or not.")]
	public bool mirroredMovement = false;

	[Tooltip("Whether the avatar is allowed to move vertically or not.")]
	public bool verticalMovement = false;

	[Tooltip("Whether the avatar's root motion is applied by other component or script.")]
	public bool externalRootMotion = false;

	[Tooltip("Whether the finger orientations are allowed or not.")]
	public bool fingerOrientations = false;

	[Tooltip("Rate at which the avatar will move through the scene.")]
	public float moveRate = 1f;

	[Tooltip("Smooth factor used for avatar movements and joint rotations.")]
	public float smoothFactor = 5f;

	[Tooltip("Game object this transform is relative to (optional).")]
	public GameObject offsetNode;

	[Tooltip("If enabled, makes the avatar position relative to this camera to be the same as the player's position to the sensor.")]
	public Camera posRelativeToCamera;

	[Tooltip("Whether the avatar's position should match the color image (in Pos-rel-to-camera mode only).")]
	public bool posRelOverlayColor = false;

	[Tooltip("Whether z-axis movement needs to be inverted (Pos-Relative mode only).")]
	public bool posRelInvertedZ = false;

	//	[Tooltip("Whether the avatar position overlays the color camera background or not.")]
	//	protected bool avatarPosOverlaysBackground = true;

	// userId of the player
	[NonSerialized]
	public Int64 playerId = 0;


	// The body root node
	protected Transform bodyRoot;

	// Variable to hold all them bones. It will initialize the same size as initialRotations.
	protected Transform[] bones;

	// Rotations of the bones when the Kinect tracking starts.
	protected Quaternion[] initialRotations;

	// Initial position and rotation of the transform
	protected Vector3 initialPosition;
	protected Quaternion initialRotation;
	protected Vector3 offsetNodePos;
	protected Quaternion offsetNodeRot;
	protected Vector3 bodyRootPosition;

	// Calibration Offset Variables for Character Position.
	protected bool offsetCalibrated = false;
	protected Vector3 offsetPos = Vector3.zero;
	//protected float xOffset, yOffset, zOffset;
	//private Quaternion originalRotation;

	// whether the parent transform obeys physics
	protected bool isRigidBody = false;

	// private instance of the KinectManager
	protected KinectManager kinectManager;

	// last hand events
	private InteractionManager.HandEventType lastLeftHandEvent = InteractionManager.HandEventType.Release;
	private InteractionManager.HandEventType lastRightHandEvent = InteractionManager.HandEventType.Release;

	// fist states
	private bool bLeftFistDone = false;
	private bool bRightFistDone = false;


	//position for judge wheter user is jumping or jogging
	private float originLeftFootJointPosition = 0.0f;
	private float currentLeftFootJointPosition = 0.0f;
	
	private float originRightFootJointPosition = 0.0f;
	private float currentRightFootJointPosition = 0.0f;

	private float currentLeftFootJointSpeed = 0.0f;
	private float currentRightFootJointSpeed = 0.0f;

	private float currentHeadJointPostion = 0.0f;
	private float currentKneeJointPositon = 0.0f;

	private float originShouldJointPosition = 0.0f;

	private float currentShouldJointPositon = 0.0f;

	/// <summary>
	/// Gets the number of bone transforms (array length).
	/// </summary>
	/// <returns>The number of bone transforms.</returns>
	public int GetBoneTransformCount()
	{
		return bones != null ? bones.Length : 0;
	}

	/// <summary>
	/// Gets the bone transform by index.
	/// </summary>
	/// <returns>The bone transform.</returns>
	/// <param name="index">Index</param>
	public Transform GetBoneTransform(int index)
	{
		if(index >= 0 && index < bones.Length)
		{
			return bones[index];
		}

		return null;
	}

	/// <summary>
	/// Gets the bone index by joint type.
	/// </summary>
	/// <returns>The bone index.</returns>
	/// <param name="joint">Joint type</param>
	/// <param name="bMirrored">If set to <c>true</c> gets the mirrored joint index.</param>
	public int GetBoneIndexByJoint(KinectInterop.JointType joint, bool bMirrored)
	{
		int boneIndex = -1;

		if(jointMap2boneIndex.ContainsKey(joint))
		{
			boneIndex = !bMirrored ? jointMap2boneIndex[joint] : mirrorJointMap2boneIndex[joint];
		}

		return boneIndex;
	}

	/// <summary>
	/// Gets the special index by two joint types.
	/// </summary>
	/// <returns>The spec index by joint.</returns>
	/// <param name="joint1">Joint 1 type.</param>
	/// <param name="joint2">Joint 2 type.</param>
	/// <param name="bMirrored">If set to <c>true</c> gets the mirrored joint index.</param>
	public int GetSpecIndexByJoint(KinectInterop.JointType joint1, KinectInterop.JointType joint2, bool bMirrored)
	{
		int boneIndex = -1;

		if((joint1 == KinectInterop.JointType.ShoulderLeft && joint2 == KinectInterop.JointType.SpineShoulder) ||
			(joint2 == KinectInterop.JointType.ShoulderLeft && joint1 == KinectInterop.JointType.SpineShoulder))
		{
			return (!bMirrored ? 25 : 26);
		}
		else if((joint1 == KinectInterop.JointType.ShoulderRight && joint2 == KinectInterop.JointType.SpineShoulder) ||
			(joint2 == KinectInterop.JointType.ShoulderRight && joint1 == KinectInterop.JointType.SpineShoulder))
		{
			return (!bMirrored ? 26 : 25);
		}
		else if((joint1 == KinectInterop.JointType.HandTipLeft && joint2 == KinectInterop.JointType.HandLeft) ||
			(joint2 == KinectInterop.JointType.HandTipLeft && joint1 == KinectInterop.JointType.HandLeft))
		{
			return (!bMirrored ? 27 : 28);
		}
		else if((joint1 == KinectInterop.JointType.HandTipRight && joint2 == KinectInterop.JointType.HandRight) ||
			(joint2 == KinectInterop.JointType.HandTipRight && joint1 == KinectInterop.JointType.HandRight))
		{
			return (!bMirrored ? 28 : 27);
		}
		else if((joint1 == KinectInterop.JointType.ThumbLeft && joint2 == KinectInterop.JointType.HandLeft) ||
			(joint2 == KinectInterop.JointType.ThumbLeft && joint1 == KinectInterop.JointType.HandLeft))
		{
			return (!bMirrored ? 29 : 30);
		}
		else if((joint1 == KinectInterop.JointType.ThumbRight && joint2 == KinectInterop.JointType.HandRight) ||
			(joint2 == KinectInterop.JointType.ThumbRight && joint1 == KinectInterop.JointType.HandRight))
		{
			return (!bMirrored ? 30 : 29);
		}

		return boneIndex;
	}


	// transform caching gives performance boost since Unity calls GetComponent<Transform>() each time you call transform 
	private Transform _transformCache;
	public new Transform transform
	{
		get
		{
			if (!_transformCache) 
			{
				_transformCache = base.transform;
			}

			return _transformCache;
		}
	}


	public void Awake()
	{	
		// check for double start
		if(bones != null)
			return;
		if(!gameObject.activeInHierarchy) 
			return;

		// Set model's arms to be in T-pose, if needed
		SetModelArmsInTpose();

		// inits the bones array
		bones = new Transform[31];

		// Initial rotations and directions of the bones.
		initialRotations = new Quaternion[bones.Length];

		// Map bones to the points the Kinect tracks
		MapBones();

		// Get initial bone rotations
		GetInitialRotations();

		// if parent transform uses physics
		isRigidBody = gameObject.GetComponent<Rigidbody>();
		
		resetPositionInfo_();
	}

	/// <summary>
	/// Updates the avatar each frame.
	/// </summary>
	/// <param name="UserID">User ID</param>
	public void UpdateAvatar(Int64 UserID)
	{	
		if(!gameObject.activeInHierarchy) 
			return;

		// Get the KinectManager instance
		if(kinectManager == null)
		{
			kinectManager = KinectManager.Instance;
		}

		// move the avatar to its Kinect position
		if(!externalRootMotion)
		{
			MoveAvatar(UserID);
		}

		// get the left hand state and event
		if(kinectManager && kinectManager.GetJointTrackingState(UserID, (int)KinectInterop.JointType.HandLeft) != KinectInterop.TrackingState.NotTracked)
		{
			KinectInterop.HandState leftHandState = kinectManager.GetLeftHandState(UserID);
			InteractionManager.HandEventType leftHandEvent = InteractionManager.HandStateToEvent(leftHandState, lastLeftHandEvent);

			if(leftHandEvent != InteractionManager.HandEventType.None)
			{
				lastLeftHandEvent = leftHandEvent;
			}
		}

		// get the right hand state and event
		if(kinectManager && kinectManager.GetJointTrackingState(UserID, (int)KinectInterop.JointType.HandRight) != KinectInterop.TrackingState.NotTracked)
		{
			KinectInterop.HandState rightHandState = kinectManager.GetRightHandState(UserID);
			InteractionManager.HandEventType rightHandEvent = InteractionManager.HandStateToEvent(rightHandState, lastRightHandEvent);

			if(rightHandEvent != InteractionManager.HandEventType.None)
			{
				lastRightHandEvent = rightHandEvent;
			}
		}

		// rotate the avatar bones
		for (var boneIndex = 0; boneIndex < bones.Length; boneIndex++)
		{
			if (!bones[boneIndex]) 
				continue;

			if(boneIndex2JointMap.ContainsKey(boneIndex))
			{
				KinectInterop.JointType joint = !mirroredMovement ? boneIndex2JointMap[boneIndex] : boneIndex2MirrorJointMap[boneIndex];
				TransformBone(UserID, joint, boneIndex, !mirroredMovement);
			}
			else if(specIndex2JointMap.ContainsKey(boneIndex))
			{
				// special bones (clavicles)
				List<KinectInterop.JointType> alJoints = !mirroredMovement ? specIndex2JointMap[boneIndex] : specIndex2MirrorMap[boneIndex];

				if(alJoints.Count >= 2)
				{
					//Debug.Log(alJoints[0].ToString());
					Vector3 baseDir = alJoints[0].ToString().EndsWith("Left") ? Vector3.left : Vector3.right;
					TransformSpecialBone(UserID, alJoints[0], alJoints[1], boneIndex, baseDir, !mirroredMovement);
				}
			}
		}
		
		//update relative position info
		updatePositionInfo_(UserID);
	}

	/// <summary>
	/// Resets bones to their initial positions and rotations.
	/// </summary>
	public void ResetToInitialPosition()
	{
		playerId = 0;

		if(bones == null)
			return;

		// For each bone that was defined, reset to initial position.
		transform.rotation = Quaternion.identity;

		for(int pass = 0; pass < 2; pass++)  // 2 passes because clavicles are at the end
		{
			for(int i = 0; i < bones.Length; i++)
			{
				if(bones[i] != null)
				{
					bones[i].rotation = initialRotations[i];
				}
			}
		}

		//		if(bodyRoot != null)
		//		{
		//			bodyRoot.localPosition = Vector3.zero;
		//			bodyRoot.localRotation = Quaternion.identity;
		//		}

		// Restore the offset's position and rotation
		if(offsetNode != null)
		{
			offsetNode.transform.position = offsetNodePos;
			offsetNode.transform.rotation = offsetNodeRot;
		}

		transform.position = initialPosition;
		transform.rotation = initialRotation;
		
		//reset relative position info
		resetPositionInfo_();
	}

	/// <summary>
	/// Invoked on the successful calibration of the player.
	/// </summary>
	/// <param name="userId">User identifier.</param>
	public void SuccessfulCalibration(Int64 userId)
	{
		playerId = userId;

		// reset the models position
		if(offsetNode != null)
		{
			offsetNode.transform.position = offsetNodePos;
			offsetNode.transform.rotation = offsetNodeRot;
		}

		transform.position = initialPosition;
		transform.rotation = initialRotation;

		// re-calibrate the position offset
		offsetCalibrated = false;
	}

	// Apply the rotations tracked by kinect to the joints.
	protected void TransformBone(Int64 userId, KinectInterop.JointType joint, int boneIndex, bool flip)
	{
		Transform boneTransform = bones[boneIndex];
		if(boneTransform == null || kinectManager == null)
			return;

		int iJoint = (int)joint;
		if(iJoint < 0 || !kinectManager.IsJointTracked(userId, iJoint))
			return;

		// Get Kinect joint orientation
		Quaternion jointRotation = kinectManager.GetJointOrientation(userId, iJoint, flip);
		if(jointRotation == Quaternion.identity)
			return;

		// calculate the new orientation
		Quaternion newRotation = Kinect2AvatarRot(jointRotation, boneIndex);

		if(externalRootMotion)
		{
			newRotation = transform.rotation * newRotation;
		}

		// Smoothly transition to the new rotation
		if(smoothFactor != 0f)
			boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, newRotation, smoothFactor * Time.deltaTime);
		else
			boneTransform.rotation = newRotation;
	}

	// Apply the rotations tracked by kinect to a special joint
	protected void TransformSpecialBone(Int64 userId, KinectInterop.JointType joint, KinectInterop.JointType jointParent, int boneIndex, Vector3 baseDir, bool flip)
	{
		Transform boneTransform = bones[boneIndex];
		if(boneTransform == null || kinectManager == null)
			return;

		if(!kinectManager.IsJointTracked(userId, (int)joint) || 
			!kinectManager.IsJointTracked(userId, (int)jointParent))
		{
			return;
		}

		if(boneIndex >= 27 && boneIndex <= 30)
		{
			// fingers or thumbs
			if(fingerOrientations)
			{
				TransformSpecialBoneFingers(userId, (int)joint, boneIndex, flip);
			}

			return;
		}

		Vector3 jointDir = kinectManager.GetJointDirection(userId, (int)joint, false, true);
		Quaternion jointRotation = jointDir != Vector3.zero ? Quaternion.FromToRotation(baseDir, jointDir) : Quaternion.identity;

		if(!flip)
		{
			Vector3 mirroredAngles = jointRotation.eulerAngles;
			mirroredAngles.y = -mirroredAngles.y;
			mirroredAngles.z = -mirroredAngles.z;

			jointRotation = Quaternion.Euler(mirroredAngles);
		}

		if(jointRotation != Quaternion.identity)
		{
			// Smoothly transition to the new rotation
			Quaternion newRotation = Kinect2AvatarRot(jointRotation, boneIndex);

			if(externalRootMotion)
			{
				newRotation = transform.rotation * newRotation;
			}

			if(smoothFactor != 0f)
				boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, newRotation, smoothFactor * Time.deltaTime);
			else
				boneTransform.rotation = newRotation;
		}

	}

	// Apply the rotations tracked by kinect to fingers (one joint = multiple bones)
	protected void TransformSpecialBoneFingers(Int64 userId, int joint, int boneIndex, bool flip)
	{
		// check for closed hands
		if(joint == (int)KinectInterop.JointType.HandTipLeft || joint == (int)KinectInterop.JointType.ThumbLeft)
		{
			if(lastLeftHandEvent == InteractionManager.HandEventType.Grip)
			{
				if(!bLeftFistDone)
				{
					float angleSign = (boneIndex == 27 || boneIndex == 29) ? 1f : -1f;
					float angleRot = angleSign * (joint == (int)KinectInterop.JointType.HandTipLeft ? 80f : 60f);

					TransformSpecialBoneFist(boneIndex, angleRot);
					bLeftFistDone = (boneIndex >= 29);
				}
				return;
			}
			else if(bLeftFistDone && lastLeftHandEvent == InteractionManager.HandEventType.Release)
			{
				bLeftFistDone = false;
			}
		}
		else if(joint == (int)KinectInterop.JointType.HandTipRight || joint == (int)KinectInterop.JointType.ThumbRight)
		{
			if(lastRightHandEvent == InteractionManager.HandEventType.Grip)
			{
				if(!bRightFistDone)
				{
					float angleSign = (boneIndex == 27 || boneIndex == 29) ? 1f : -1f;
					float angleRot = angleSign * (joint == (int)KinectInterop.JointType.HandTipRight ? 80f : 60f);

					TransformSpecialBoneFist(boneIndex, angleRot);
					bRightFistDone = (boneIndex >= 29);
				}
				return;
			}
			else if(bRightFistDone && lastRightHandEvent == InteractionManager.HandEventType.Release)
			{
				bRightFistDone = false;
			}
		}

		// get the animator component
		Animator animatorComponent = GetComponent<Animator>();
		if(!animatorComponent)
			return;

		// Get Kinect joint orientation
		Quaternion jointRotation = kinectManager.GetJointOrientation(userId, joint, flip);
		if(jointRotation == Quaternion.identity)
			return;

		// calculate the new orientation
		Quaternion newRotation = Kinect2AvatarRot(jointRotation, boneIndex);

		if(externalRootMotion)
		{
			newRotation = transform.rotation * newRotation;
		}

		// get the list of bones
		//List<HumanBodyBones> alBones = flip ? specialIndex2MultiBoneMap[boneIndex] : specialIndex2MirrorBoneMap[boneIndex];
		List<HumanBodyBones> alBones = specialIndex2MultiBoneMap[boneIndex];

		// Smoothly transition to the new rotation
		for(int i = 0; i < alBones.Count; i++)
		{
			Transform boneTransform = animatorComponent.GetBoneTransform(alBones[i]);
			if(!boneTransform)
				continue;

			if(smoothFactor != 0f)
				boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, newRotation, smoothFactor * Time.deltaTime);
			else
				boneTransform.rotation = newRotation;
		}
	}

	// Apply the rotations needed to transform fingers to fist
	protected void TransformSpecialBoneFist(int boneIndex, float angle)
	{
		//		// do fist only for fingers
		//		if(boneIndex != 27 && boneIndex != 28)
		//			return;
		//
		//		// get the animator component
		//		Animator animatorComponent = GetComponent<Animator>();
		//		if(!animatorComponent)
		//			return;
		//		
		//		// get the list of bones
		//		//List<HumanBodyBones> alBones = flip ? specialIndex2MultiBoneMap[boneIndex] : specialIndex2MirrorBoneMap[boneIndex];
		//		List<HumanBodyBones> alBones = specialIndex2MultiBoneMap[boneIndex];
		//		List<HumanBodyBones> alOrtho = specialIndex2MultiBoneMap[(boneIndex == 27 || boneIndex == 28) ? (boneIndex + 2) : (boneIndex - 2)];
		//		
		//		if(alBones.Count > 0 && alOrtho.Count > 0)
		//		{
		//			Transform handTransform = animatorComponent.GetBoneTransform((boneIndex == 27 || boneIndex == 29) ? HumanBodyBones.LeftHand : HumanBodyBones.RightHand);
		//			Transform orthoTransform = animatorComponent.GetBoneTransform(alOrtho[0]);
		//			Transform oparTransform = orthoTransform ? orthoTransform.parent : null;
		//
		//			for(int i = 0; i < alBones.Count; i++)
		//			{
		//				Transform boneTransform = animatorComponent.GetBoneTransform(alBones[i]);
		//				Transform bparTransform = boneTransform ? boneTransform.parent : null;
		//
		//				if(!boneTransform || !bparTransform)
		//					continue;
		//
		//				// find the fist rotation
		//				Transform fingerTransform = animatorComponent.GetBoneTransform(alBones[i - (i % 3)]);
		//
		//				if(fingerTransform && handTransform && orthoTransform && oparTransform)
		//				//if(handTransform && orthoTransform && oparTransform)
		//				{
		//					Vector3 vBoneDir = fingerTransform.position - handTransform.position;
		//					//Vector3 vBoneDir = bparTransform.position - handTransform.position;
		//
		//					Vector3 vOrthoDir = (boneIndex == 27 || boneIndex == 28) ? // fingers or thumbs?
		//							(orthoTransform.position - oparTransform.position) : 
		//							(oparTransform.position - orthoTransform.position);
		//
		//					Vector3.OrthoNormalize(ref vBoneDir, ref vOrthoDir);
		//
		//					Vector3 vOrthoLclDir = boneTransform.InverseTransformDirection(vOrthoDir);
		//					Quaternion qRotFinger = Quaternion.AngleAxis(angle, vOrthoLclDir);
		//
		//					// Smoothly transition to the new rotation
		////					if(smoothFactor != 0f)
		////						boneTransform.localRotation = Quaternion.Slerp(boneTransform.localRotation, qRotFinger, smoothFactor * Time.deltaTime);
		////					else
		//						boneTransform.localRotation = qRotFinger;
		//				}
		//			}
		//
		//		}
	}

	// Moves the avatar - gets the tracked position of the user and applies it to avatar.
	protected void MoveAvatar(Int64 UserID)
	{
		if((moveRate == 0f) || !kinectManager ||
			!kinectManager.IsJointTracked(UserID, (int)KinectInterop.JointType.SpineBase))
		{
			return;
		}

		// get the position of user's spine base
		Vector3 trans = kinectManager.GetUserPosition(UserID);

		// use the color overlay position if needed
		if(posRelativeToCamera && posRelOverlayColor)
		{
			Rect backgroundRect = posRelativeToCamera.pixelRect;
			PortraitBackground portraitBack = PortraitBackground.Instance;

			if(portraitBack && portraitBack.enabled)
			{
				backgroundRect = portraitBack.GetBackgroundRect();
			}

			trans = kinectManager.GetJointPosColorOverlay(UserID, (int)KinectInterop.JointType.SpineBase, posRelativeToCamera, backgroundRect);
		}

		// invert the z-coordinate, if needed
		if(posRelativeToCamera && posRelInvertedZ)
		{
			trans.z = -trans.z;
		}

		//		if(posRelativeToCamera && avatarPosOverlaysBackground)
		//		{
		//			// gets the user's spine-base position, matching the color-camera background
		//			Rect backgroundRect = posRelativeToCamera.pixelRect;
		//			PortraitBackground portraitBack = PortraitBackground.Instance;
		//			
		//			if(portraitBack && portraitBack.enabled)
		//			{
		//				backgroundRect = portraitBack.GetBackgroundRect();
		//			}
		//
		//			trans = kinectManager.GetJointPosColorOverlay(UserID, (int)KinectInterop.JointType.SpineBase, posRelativeToCamera, backgroundRect);
		//		}

		if(!offsetCalibrated)
		{
			offsetCalibrated = true;

			offsetPos.x = trans.x;  // !mirroredMovement ? trans.x * moveRate : -trans.x * moveRate;
			offsetPos.y = trans.y;  // trans.y * moveRate;
			offsetPos.z = !mirroredMovement && !posRelativeToCamera ? -trans.z : trans.z;  // -trans.z * moveRate;

			if(posRelativeToCamera)
			{
				Vector3 cameraPos = posRelativeToCamera.transform.position;
				Vector3 bodyRootPos = bodyRoot != null ? bodyRoot.position : transform.position;
				Vector3 hipCenterPos = bodyRoot != null ? bodyRoot.position : bones[0].position;

				float yRelToAvatar = 0f;
				if(verticalMovement)
				{
					yRelToAvatar = (trans.y - cameraPos.y) - (hipCenterPos - bodyRootPos).magnitude;
				}
				else
				{
					yRelToAvatar = bodyRootPos.y - cameraPos.y;
				}

				Vector3 relativePos = new Vector3(trans.x, yRelToAvatar, trans.z);
				Vector3 newBodyRootPos = cameraPos + relativePos;

				//				if(offsetNode != null)
				//				{
				//					newBodyRootPos += offsetNode.transform.position;
				//				}

				if(bodyRoot != null)
				{
					bodyRoot.position = newBodyRootPos;
				}
				else
				{
					transform.position = newBodyRootPos;
				}

				bodyRootPosition = newBodyRootPos;
			}
		}

		// transition to the new position
		Vector3 targetPos = bodyRootPosition + Kinect2AvatarPos(trans, verticalMovement);

		if(isRigidBody && !verticalMovement)
		{
			// workaround for obeying the physics (e.g. gravity falling)
			targetPos.y = bodyRoot != null ? bodyRoot.position.y : transform.position.y;
		}

		if(bodyRoot != null)
		{
			bodyRoot.position = smoothFactor != 0f ? 
				Vector3.Lerp(bodyRoot.position, targetPos, smoothFactor * Time.deltaTime) : targetPos;
		}
		else
		{
			transform.position = smoothFactor != 0f ? 
				Vector3.Lerp(transform.position, targetPos, smoothFactor * Time.deltaTime) : targetPos;
		}
	}

	// Set model's arms to be in T-pose
	protected void SetModelArmsInTpose()
	{
		Vector3 vTposeLeftDir = transform.TransformDirection(Vector3.left);
		Vector3 vTposeRightDir = transform.TransformDirection(Vector3.right);
		Animator animator = GetComponent<Animator>();

		Transform transLeftUarm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
		Transform transLeftLarm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
		Transform transLeftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);

		if(transLeftUarm != null && transLeftLarm != null)
		{
			Vector3 vUarmLeftDir = transLeftLarm.position - transLeftUarm.position;
			float fUarmLeftAngle = Vector3.Angle(vUarmLeftDir, vTposeLeftDir);

			if(Mathf.Abs(fUarmLeftAngle) >= 5f)
			{
				Quaternion vFixRotation = Quaternion.FromToRotation(vUarmLeftDir, vTposeLeftDir);
				transLeftUarm.rotation = vFixRotation * transLeftUarm.rotation;
			}

			if(transLeftHand != null)
			{
				Vector3 vLarmLeftDir = transLeftHand.position - transLeftLarm.position;
				float fLarmLeftAngle = Vector3.Angle(vLarmLeftDir, vTposeLeftDir);

				if(Mathf.Abs(fLarmLeftAngle) >= 5f)
				{
					Quaternion vFixRotation = Quaternion.FromToRotation(vLarmLeftDir, vTposeLeftDir);
					transLeftLarm.rotation = vFixRotation * transLeftLarm.rotation;
				}
			}
		}

		Transform transRightUarm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
		Transform transRightLarm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
		Transform transRightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);

		if(transRightUarm != null && transRightLarm != null)
		{
			Vector3 vUarmRightDir = transRightLarm.position - transRightUarm.position;
			float fUarmRightAngle = Vector3.Angle(vUarmRightDir, vTposeRightDir);

			if(Mathf.Abs(fUarmRightAngle) >= 5f)
			{
				Quaternion vFixRotation = Quaternion.FromToRotation(vUarmRightDir, vTposeRightDir);
				transRightUarm.rotation = vFixRotation * transRightUarm.rotation;
			}

			if(transRightHand != null)
			{
				Vector3 vLarmRightDir = transRightHand.position - transRightLarm.position;
				float fLarmRightAngle = Vector3.Angle(vLarmRightDir, vTposeRightDir);

				if(Mathf.Abs(fLarmRightAngle) >= 5f)
				{
					Quaternion vFixRotation = Quaternion.FromToRotation(vLarmRightDir, vTposeRightDir);
					transRightLarm.rotation = vFixRotation * transRightLarm.rotation;
				}
			}
		}

	}

	// If the bones to be mapped have been declared, map that bone to the model.
	protected virtual void MapBones()
	{
		//		// make OffsetNode as a parent of model transform.
		//		offsetNode = new GameObject(name + "Ctrl") { layer = transform.gameObject.layer, tag = transform.gameObject.tag };
		//		offsetNode.transform.position = transform.position;
		//		offsetNode.transform.rotation = transform.rotation;
		//		offsetNode.transform.parent = transform.parent;

		//		// take model transform as body root
		//		transform.parent = offsetNode.transform;
		//		transform.localPosition = Vector3.zero;
		//		transform.localRotation = Quaternion.identity;

		//bodyRoot = transform;

		// get bone transforms from the animator component
		Animator animatorComponent = GetComponent<Animator>();

		for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
		{
			if (!boneIndex2MecanimMap.ContainsKey(boneIndex)) 
				continue;

			bones[boneIndex] = animatorComponent ? animatorComponent.GetBoneTransform(boneIndex2MecanimMap[boneIndex]) : null;
		}
	}

	// Capture the initial rotations of the bones
	protected void GetInitialRotations()
	{
		// save the initial rotation
		if(offsetNode != null)
		{
			offsetNodePos = offsetNode.transform.position;
			offsetNodeRot = offsetNode.transform.rotation;
		}

		initialPosition = transform.position;
		initialRotation = transform.rotation;

		//		if(offsetNode != null)
		//		{
		//			initialRotation = Quaternion.Inverse(offsetNodeRot) * initialRotation;
		//		}

		transform.rotation = Quaternion.identity;

		// save the body root initial position
		if(bodyRoot != null)
		{
			bodyRootPosition = bodyRoot.position;
		}
		else
		{
			bodyRootPosition = transform.position;
		}

		if(offsetNode != null)
		{
			bodyRootPosition = bodyRootPosition - offsetNodePos;
		}

		// save the initial bone rotations
		for (int i = 0; i < bones.Length; i++)
		{
			if (bones[i] != null)
			{
				initialRotations[i] = bones[i].rotation;
			}
		}

		// Restore the initial rotation
		transform.rotation = initialRotation;
	}

	// Converts kinect joint rotation to avatar joint rotation, depending on joint initial rotation and offset rotation
	protected Quaternion Kinect2AvatarRot(Quaternion jointRotation, int boneIndex)
	{
		Quaternion newRotation = jointRotation * initialRotations[boneIndex];
		//newRotation = initialRotation * newRotation;

		if(offsetNode != null)
		{
			newRotation = offsetNode.transform.rotation * newRotation;
		}
		else
		{
			newRotation = initialRotation * newRotation;
		}

		return newRotation;
	}

	// Converts Kinect position to avatar skeleton position, depending on initial position, mirroring and move rate
	protected Vector3 Kinect2AvatarPos(Vector3 jointPosition, bool bMoveVertically)
	{
		float xPos = (jointPosition.x - offsetPos.x) * moveRate;
		float yPos = (jointPosition.y - offsetPos.y) * moveRate;
		float zPos = !mirroredMovement && !posRelativeToCamera ? (-jointPosition.z - offsetPos.z) * moveRate : (jointPosition.z - offsetPos.z) * moveRate;

		Vector3 newPosition = new Vector3(xPos, bMoveVertically ? yPos : 0f, zPos);

		if(offsetNode != null)
		{
			newPosition += offsetNode.transform.position;
		}

		return newPosition;
	}
	
	//reset the position info to zero
	private void resetPositionInfo_(){
		//reset foot's speed and positon
		originLeftFootJointPosition  = originRightFootJointPosition 
			= currentLeftFootJointPosition = currentRightFootJointPosition = currentRightFootJointSpeed 
			= currentHeadJointPostion = currentKneeJointPositon = originShouldJointPosition = currentShouldJointPositon = 0.0f;
	}
	
	//set the speed value = offset / time = (currentPos - originPos) / time
	private void updatePositionInfo_(Int64 userID){
		if(kinectManager != null){
			float temp1 = currentLeftFootJointPosition;
			float temp2 = currentRightFootJointPosition;
			float temp3 = currentShouldJointPositon;
			//update left foot joint
			if(kinectManager.IsJointTracked(userID, (int)KinectInterop.JointType.FootLeft)){
				// output the joint position for easy tracking
				currentLeftFootJointPosition = Kinect2AvatarPos(kinectManager.GetJointPosition(userID, (int)KinectInterop.JointType.FootLeft),true).y;
			}
			//update right foot joint
			if(kinectManager.IsJointTracked(userID, (int)KinectInterop.JointType.FootRight)){
				// output the joint position for easy tracking
				currentRightFootJointPosition = Kinect2AvatarPos(kinectManager.GetJointPosition(userID, (int)KinectInterop.JointType.FootRight),true).y;
			}
			
			//update head joint
			if(kinectManager.IsJointTracked(userID, (int)KinectInterop.JointType.Head)){
				// output the joint position for easy tracking
				currentHeadJointPostion = Kinect2AvatarPos(kinectManager.GetJointPosition(userID, (int)KinectInterop.JointType.Head),true).y;
			}
			
			//update right knee joint
			if(kinectManager.IsJointTracked(userID, (int)KinectInterop.JointType.KneeRight)){
				// output the joint position for easy tracking
				currentKneeJointPositon = Kinect2AvatarPos(kinectManager.GetJointPosition(userID, (int)KinectInterop.JointType.KneeRight),true).y;
			}

			if(kinectManager.IsJointTracked(userID,(int)(KinectInterop.JointType.SpineMid))){
				currentShouldJointPositon = Kinect2AvatarPos(kinectManager.GetJointPosition(userID,(int)KinectInterop.JointType.SpineMid),true).x;
			}
			
			//since refresh  rate = 30/s, thus divide 1 / 30 = * 30;
			currentLeftFootJointSpeed = (currentLeftFootJointPosition - originLeftFootJointPosition) * 30;
			currentRightFootJointSpeed = (currentRightFootJointPosition - originRightFootJointPosition) *30; 
			
			originLeftFootJointPosition = temp1;
			originRightFootJointPosition = temp2;
			originShouldJointPosition = temp3;
		}
	}
	
	public bool isMoving(){
		return (System.Math.Abs(currentLeftFootJointSpeed) > 0.2) && (System.Math.Abs(currentRightFootJointSpeed) > 0.2);
	}
	
	public bool isJumping(){
		return currentLeftFootJointPosition > -0.85 && currentRightFootJointPosition > -0.85 && currentHeadJointPostion > 0.65;
	}
	
	public bool isCrouch(){
		return currentHeadJointPostion < 0.3 && currentKneeJointPositon < 0;
		//return true;
	}

	public float deltaPosition(){
		float temp = currentShouldJointPositon - originShouldJointPosition;
		if(System.Math.Abs(temp) > 0.04)
			return temp;
		else
			return 0.0f;
	}


	// void OnGUI(){
	// 	if(isMoving())
	// 		GUI.Label (new Rect (Screen.width / 2, 60, 150, 150), currentRightFootJointSpeed.ToString() + " "  + currentLeftFootJointSpeed.ToString());
		
	// }

	//	protected void OnCollisionEnter(Collision col)
	//	{
	//		Debug.Log("Collision entered");
	//	}
	//
	//	protected void OnCollisionExit(Collision col)
	//	{
	//		Debug.Log("Collision exited");
	//	}

	// dictionaries to speed up bones' processing
	// the author of the terrific idea for kinect-joints to mecanim-bones mapping
	// along with its initial implementation, including following dictionary is
	// Mikhail Korchun (korchoon@gmail.com). Big thanks to this guy!
	private readonly Dictionary<int, HumanBodyBones> boneIndex2MecanimMap = new Dictionary<int, HumanBodyBones>
	{
		{0, HumanBodyBones.Hips},
		{1, HumanBodyBones.Spine},
		//        {2, HumanBodyBones.Chest},
		{3, HumanBodyBones.Neck},
		//		{4, HumanBodyBones.Head},

		{5, HumanBodyBones.LeftUpperArm},
		{6, HumanBodyBones.LeftLowerArm},
		{7, HumanBodyBones.LeftHand},
		//		{8, HumanBodyBones.LeftIndexProximal},
		//		{9, HumanBodyBones.LeftIndexIntermediate},
		//		{10, HumanBodyBones.LeftThumbProximal},

		{11, HumanBodyBones.RightUpperArm},
		{12, HumanBodyBones.RightLowerArm},
		{13, HumanBodyBones.RightHand},
		//		{14, HumanBodyBones.RightIndexProximal},
		//		{15, HumanBodyBones.RightIndexIntermediate},
		//		{16, HumanBodyBones.RightThumbProximal},

		{17, HumanBodyBones.LeftUpperLeg},
		{18, HumanBodyBones.LeftLowerLeg},
		{19, HumanBodyBones.LeftFoot},
		//		{20, HumanBodyBones.LeftToes},

		{21, HumanBodyBones.RightUpperLeg},
		{22, HumanBodyBones.RightLowerLeg},
		{23, HumanBodyBones.RightFoot},
		//		{24, HumanBodyBones.RightToes},

		{25, HumanBodyBones.LeftShoulder},
		{26, HumanBodyBones.RightShoulder},
		{27, HumanBodyBones.LeftIndexProximal},
		{28, HumanBodyBones.RightIndexProximal},
		{29, HumanBodyBones.LeftThumbProximal},
		{30, HumanBodyBones.RightThumbProximal},
	};

	protected readonly Dictionary<int, KinectInterop.JointType> boneIndex2JointMap = new Dictionary<int, KinectInterop.JointType>
	{
		{0, KinectInterop.JointType.SpineBase},
		{1, KinectInterop.JointType.SpineMid},
		{2, KinectInterop.JointType.SpineShoulder},
		{3, KinectInterop.JointType.Neck},
		{4, KinectInterop.JointType.Head},

		{5, KinectInterop.JointType.ShoulderLeft},
		{6, KinectInterop.JointType.ElbowLeft},
		{7, KinectInterop.JointType.WristLeft},
		{8, KinectInterop.JointType.HandLeft},

		{9, KinectInterop.JointType.HandTipLeft},
		{10, KinectInterop.JointType.ThumbLeft},

		{11, KinectInterop.JointType.ShoulderRight},
		{12, KinectInterop.JointType.ElbowRight},
		{13, KinectInterop.JointType.WristRight},
		{14, KinectInterop.JointType.HandRight},

		{15, KinectInterop.JointType.HandTipRight},
		{16, KinectInterop.JointType.ThumbRight},

		{17, KinectInterop.JointType.HipLeft},
		{18, KinectInterop.JointType.KneeLeft},
		{19, KinectInterop.JointType.AnkleLeft},
		{20, KinectInterop.JointType.FootLeft},

		{21, KinectInterop.JointType.HipRight},
		{22, KinectInterop.JointType.KneeRight},
		{23, KinectInterop.JointType.AnkleRight},
		{24, KinectInterop.JointType.FootRight},
	};

	protected readonly Dictionary<int, List<KinectInterop.JointType>> specIndex2JointMap = new Dictionary<int, List<KinectInterop.JointType>>
	{
		{25, new List<KinectInterop.JointType> {KinectInterop.JointType.ShoulderLeft, KinectInterop.JointType.SpineShoulder} },
		{26, new List<KinectInterop.JointType> {KinectInterop.JointType.ShoulderRight, KinectInterop.JointType.SpineShoulder} },
		{27, new List<KinectInterop.JointType> {KinectInterop.JointType.HandTipLeft, KinectInterop.JointType.HandLeft} },
		{28, new List<KinectInterop.JointType> {KinectInterop.JointType.HandTipRight, KinectInterop.JointType.HandRight} },
		{29, new List<KinectInterop.JointType> {KinectInterop.JointType.ThumbLeft, KinectInterop.JointType.HandLeft} },
		{30, new List<KinectInterop.JointType> {KinectInterop.JointType.ThumbRight, KinectInterop.JointType.HandRight} },
	};

	protected readonly Dictionary<int, KinectInterop.JointType> boneIndex2MirrorJointMap = new Dictionary<int, KinectInterop.JointType>
	{
		{0, KinectInterop.JointType.SpineBase},
		{1, KinectInterop.JointType.SpineMid},
		{2, KinectInterop.JointType.SpineShoulder},
		{3, KinectInterop.JointType.Neck},
		{4, KinectInterop.JointType.Head},

		{5, KinectInterop.JointType.ShoulderRight},
		{6, KinectInterop.JointType.ElbowRight},
		{7, KinectInterop.JointType.WristRight},
		{8, KinectInterop.JointType.HandRight},

		{9, KinectInterop.JointType.HandTipRight},
		{10, KinectInterop.JointType.ThumbRight},

		{11, KinectInterop.JointType.ShoulderLeft},
		{12, KinectInterop.JointType.ElbowLeft},
		{13, KinectInterop.JointType.WristLeft},
		{14, KinectInterop.JointType.HandLeft},

		{15, KinectInterop.JointType.HandTipLeft},
		{16, KinectInterop.JointType.ThumbLeft},

		{17, KinectInterop.JointType.HipRight},
		{18, KinectInterop.JointType.KneeRight},
		{19, KinectInterop.JointType.AnkleRight},
		{20, KinectInterop.JointType.FootRight},

		{21, KinectInterop.JointType.HipLeft},
		{22, KinectInterop.JointType.KneeLeft},
		{23, KinectInterop.JointType.AnkleLeft},
		{24, KinectInterop.JointType.FootLeft},
	};

	protected readonly Dictionary<int, List<KinectInterop.JointType>> specIndex2MirrorMap = new Dictionary<int, List<KinectInterop.JointType>>
	{
		{25, new List<KinectInterop.JointType> {KinectInterop.JointType.ShoulderRight, KinectInterop.JointType.SpineShoulder} },
		{26, new List<KinectInterop.JointType> {KinectInterop.JointType.ShoulderLeft, KinectInterop.JointType.SpineShoulder} },
		{27, new List<KinectInterop.JointType> {KinectInterop.JointType.HandTipRight, KinectInterop.JointType.HandRight} },
		{28, new List<KinectInterop.JointType> {KinectInterop.JointType.HandTipLeft, KinectInterop.JointType.HandLeft} },
		{29, new List<KinectInterop.JointType> {KinectInterop.JointType.ThumbRight, KinectInterop.JointType.HandRight} },
		{30, new List<KinectInterop.JointType> {KinectInterop.JointType.ThumbLeft, KinectInterop.JointType.HandLeft} },
	};

	protected readonly Dictionary<KinectInterop.JointType, int> jointMap2boneIndex = new Dictionary<KinectInterop.JointType, int>
	{
		{KinectInterop.JointType.SpineBase, 0},
		{KinectInterop.JointType.SpineMid, 1},
		{KinectInterop.JointType.SpineShoulder, 2},
		{KinectInterop.JointType.Neck, 3},
		{KinectInterop.JointType.Head, 4},

		{KinectInterop.JointType.ShoulderLeft, 5},
		{KinectInterop.JointType.ElbowLeft, 6},
		{KinectInterop.JointType.WristLeft, 7},
		{KinectInterop.JointType.HandLeft, 8},

		{KinectInterop.JointType.HandTipLeft, 9},
		{KinectInterop.JointType.ThumbLeft, 10},

		{KinectInterop.JointType.ShoulderRight, 11},
		{KinectInterop.JointType.ElbowRight, 12},
		{KinectInterop.JointType.WristRight, 13},
		{KinectInterop.JointType.HandRight, 14},

		{KinectInterop.JointType.HandTipRight, 15},
		{KinectInterop.JointType.ThumbRight, 16},

		{KinectInterop.JointType.HipLeft, 17},
		{KinectInterop.JointType.KneeLeft, 18},
		{KinectInterop.JointType.AnkleLeft, 19},
		{KinectInterop.JointType.FootLeft, 20},

		{KinectInterop.JointType.HipRight, 21},
		{KinectInterop.JointType.KneeRight, 22},
		{KinectInterop.JointType.AnkleRight, 23},
		{KinectInterop.JointType.FootRight, 24},
	};

	protected readonly Dictionary<KinectInterop.JointType, int> mirrorJointMap2boneIndex = new Dictionary<KinectInterop.JointType, int>
	{
		{KinectInterop.JointType.SpineBase, 0},
		{KinectInterop.JointType.SpineMid, 1},
		{KinectInterop.JointType.SpineShoulder, 2},
		{KinectInterop.JointType.Neck, 3},
		{KinectInterop.JointType.Head, 4},

		{KinectInterop.JointType.ShoulderRight, 5},
		{KinectInterop.JointType.ElbowRight, 6},
		{KinectInterop.JointType.WristRight, 7},
		{KinectInterop.JointType.HandRight, 8},

		{KinectInterop.JointType.HandTipRight, 9},
		{KinectInterop.JointType.ThumbRight, 10},

		{KinectInterop.JointType.ShoulderLeft, 11},
		{KinectInterop.JointType.ElbowLeft, 12},
		{KinectInterop.JointType.WristLeft, 13},
		{KinectInterop.JointType.HandLeft, 14},

		{KinectInterop.JointType.HandTipLeft, 15},
		{KinectInterop.JointType.ThumbLeft, 16},

		{KinectInterop.JointType.HipRight, 17},
		{KinectInterop.JointType.KneeRight, 18},
		{KinectInterop.JointType.AnkleRight, 19},
		{KinectInterop.JointType.FootRight, 20},

		{KinectInterop.JointType.HipLeft, 21},
		{KinectInterop.JointType.KneeLeft, 22},
		{KinectInterop.JointType.AnkleLeft, 23},
		{KinectInterop.JointType.FootLeft, 24},
	};


	private readonly Dictionary<int, List<HumanBodyBones>> specialIndex2MultiBoneMap = new Dictionary<int, List<HumanBodyBones>>
	{
		{27, new List<HumanBodyBones> {  // left fingers
				HumanBodyBones.LeftIndexProximal,
				HumanBodyBones.LeftIndexIntermediate,
				HumanBodyBones.LeftIndexDistal,
				HumanBodyBones.LeftMiddleProximal,
				HumanBodyBones.LeftMiddleIntermediate,
				HumanBodyBones.LeftMiddleDistal,
				HumanBodyBones.LeftRingProximal,
				HumanBodyBones.LeftRingIntermediate,
				HumanBodyBones.LeftRingDistal,
				HumanBodyBones.LeftLittleProximal,
				HumanBodyBones.LeftLittleIntermediate,
				HumanBodyBones.LeftLittleDistal,
			}},
		{28, new List<HumanBodyBones> {  // right fingers
				HumanBodyBones.RightIndexProximal,
				HumanBodyBones.RightIndexIntermediate,
				HumanBodyBones.RightIndexDistal,
				HumanBodyBones.RightMiddleProximal,
				HumanBodyBones.RightMiddleIntermediate,
				HumanBodyBones.RightMiddleDistal,
				HumanBodyBones.RightRingProximal,
				HumanBodyBones.RightRingIntermediate,
				HumanBodyBones.RightRingDistal,
				HumanBodyBones.RightLittleProximal,
				HumanBodyBones.RightLittleIntermediate,
				HumanBodyBones.RightLittleDistal,
			}},
		{29, new List<HumanBodyBones> {  // left thumb
				HumanBodyBones.LeftThumbProximal,
				HumanBodyBones.LeftThumbIntermediate,
				HumanBodyBones.LeftThumbDistal,
			}},
		{30, new List<HumanBodyBones> {  // right thumb
				HumanBodyBones.RightThumbProximal,
				HumanBodyBones.RightThumbIntermediate,
				HumanBodyBones.RightThumbDistal,
			}},
	};

}

