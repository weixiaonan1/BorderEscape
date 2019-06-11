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

public class LoadLevelWithDelay : MonoBehaviour 
{
	[Tooltip("Seconds to wait before loading next level.")]
	public float waitSeconds = 0f;

	[Tooltip("Next level number. No level is loaded, if the number is negative.")]
	public int nextLevel = -1;

	[Tooltip("Whether to check for initialized KinectManager or not.")]
	public bool validateKinectManager = true;

	[Tooltip("GUI-Text used to display the debug messages.")]
	public GUIText debugText;

	private float timeToLoadLevel = 0f;
	private bool levelLoaded = false;


	void Start()
	{
		timeToLoadLevel = Time.realtimeSinceStartup + waitSeconds;

		if(validateKinectManager && debugText != null)
		{
			KinectManager manager = KinectManager.Instance;

			if(manager == null || !manager.IsInitialized())
			{
				debugText.GetComponent<GUIText>().text = "KinectManager is not initialized!";
				levelLoaded = true;
			}
		}
	}

	
	void Update() 
	{
		if(!levelLoaded && nextLevel >= 0)
		{
			if(Time.realtimeSinceStartup >= timeToLoadLevel)
			{
				levelLoaded = true;
				Application.LoadLevel(nextLevel);
			}
			else
			{
				float timeRest = timeToLoadLevel - Time.realtimeSinceStartup;

				if(debugText != null)
				{
					debugText.GetComponent<GUIText>().text = string.Format("Time to the next level: {0:F0} s.", timeRest);
				}
			}
		}
	}
	
}
