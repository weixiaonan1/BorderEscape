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

public class LoadLevelWhenUserDetected : MonoBehaviour 
{
	[Tooltip("User pose that will trigger the loading of next level.")]
	public KinectGestures.Gestures expectedUserPose = KinectGestures.Gestures.None;
	
	[Tooltip("Next level number. No level is loaded, if the number is negative.")]
	public int nextLevel = -1;

	[Tooltip("Whether to check for initialized KinectManager or not.")]
	public bool validateKinectManager = true;

	[Tooltip("GUI-Text used to display the debug messages.")]
	public GUIText debugText;


	private bool levelLoaded = false;
	private KinectGestures.Gestures savedCalibrationPose;


	void Start()
	{
		KinectManager manager = KinectManager.Instance;
		
		if(validateKinectManager && debugText != null)
		{
			if(manager == null || !manager.IsInitialized())
			{
				debugText.GetComponent<GUIText>().text = "KinectManager is not initialized!";
				levelLoaded = true;
			}
		}

		if(manager != null && manager.IsInitialized())
		{
			savedCalibrationPose = manager.playerCalibrationPose;
			manager.playerCalibrationPose = expectedUserPose;
		}
	}

	
	void Update() 
	{
		if(!levelLoaded && nextLevel >= 0)
		{
			KinectManager manager = KinectManager.Instance;
			
			if(manager != null && manager.IsUserDetected())
			{
				manager.playerCalibrationPose = savedCalibrationPose;

				levelLoaded = true;
				Application.LoadLevel(nextLevel);
			}
		}
	}
	
}
