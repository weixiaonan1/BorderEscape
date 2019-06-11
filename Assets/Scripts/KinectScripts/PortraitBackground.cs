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

/// <summary>
/// Sets the color background image in portrait mode. The aspect ratio of the game view should be set to 9:16 for Kinect v2 or 3:4 for Kinect v1.
/// </summary>
public class PortraitBackground : MonoBehaviour 
{
	[Tooltip("Whether to use the depth-image resolution in the calculation, instead of the color-image resolution.")]
	public bool useDepthImageResolution = false;

	private Rect backgroundRect;
	private static PortraitBackground instance = null;


	/// <summary>
	/// Gets the singleton PortraitBackground instance.
	/// </summary>
	/// <value>The PortraitBackground instance.</value>
	public static PortraitBackground Instance
	{
		get
		{
			return instance;
		}
	}
	

	/// <summary>
	/// Gets the background rectangle in pixels. This rectangle can be provided as an argument to the GetJointPosColorOverlay()-KM function.
	/// </summary>
	/// <returns>The background rectangle, in pixels</returns>
	public Rect GetBackgroundRect()
	{
		return backgroundRect;
	}


	void Start () 
	{
		KinectManager kinectManager = KinectManager.Instance;

		if(kinectManager && kinectManager.IsInitialized())
		{
			float fFactorDW = 0f;
			if(!useDepthImageResolution)
			{
				fFactorDW = (float)kinectManager.GetColorImageWidth() / (float)kinectManager.GetColorImageHeight() -
					(float)kinectManager.GetColorImageHeight() / (float)kinectManager.GetColorImageWidth();
			}
			else
			{
				fFactorDW = (float)kinectManager.GetDepthImageWidth() / (float)kinectManager.GetDepthImageHeight() -
					(float)kinectManager.GetDepthImageHeight() / (float)kinectManager.GetDepthImageWidth();
			}

			float fDeltaWidth = (float)Screen.height * fFactorDW;
			float dOffsetX = -fDeltaWidth / 2f;

			float fFactorSW = 0f;
			if(!useDepthImageResolution)
			{
				fFactorSW = (float)kinectManager.GetColorImageWidth() / (float)kinectManager.GetColorImageHeight();
			}
			else
			{
				fFactorSW = (float)kinectManager.GetDepthImageWidth() / (float)kinectManager.GetDepthImageHeight();
			}

			float fScreenWidth = (float)Screen.height * fFactorSW;

			GUITexture guiTexture = GetComponent<GUITexture>();
			if(guiTexture)
			{
				guiTexture.pixelInset = new Rect(dOffsetX, 0, fDeltaWidth, 0);
			}

			backgroundRect = new Rect(dOffsetX, 0, fScreenWidth, Screen.height);
			instance = this;
		}
	}
}
