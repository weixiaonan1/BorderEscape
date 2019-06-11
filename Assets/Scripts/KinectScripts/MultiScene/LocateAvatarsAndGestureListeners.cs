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

public class LocateAvatarsAndGestureListeners : MonoBehaviour 
{

	void Start () 
	{
		KinectManager manager = KinectManager.Instance;
		
		if(manager)
		{
			// remove all users, filters and avatar controllers
			manager.avatarControllers.Clear();
			manager.ClearKinectUsers();

			// get the mono scripts. avatar controllers and gesture listeners are among them
			MonoBehaviour[] monoScripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
			
			// locate the available avatar controllers
			foreach(MonoBehaviour monoScript in monoScripts)
			{
				if(typeof(AvatarController).IsAssignableFrom(monoScript.GetType()) &&
				   monoScript.enabled)
				{
					AvatarController avatar = (AvatarController)monoScript;
					manager.avatarControllers.Add(avatar);
				}
			}

			// locate Kinect gesture manager, if any
			manager.gestureManager = null;
			foreach(MonoBehaviour monoScript in monoScripts)
			{
				if(typeof(KinectGestures).IsAssignableFrom(monoScript.GetType()) && 
				   monoScript.enabled)
				{
					manager.gestureManager = (KinectGestures)monoScript;
					break;
				}
			}

			// locate the available gesture listeners
			manager.gestureListeners.Clear();

			foreach(MonoBehaviour monoScript in monoScripts)
			{
				if(typeof(KinectGestures.GestureListenerInterface).IsAssignableFrom(monoScript.GetType()) &&
				   monoScript.enabled)
				{
					//KinectGestures.GestureListenerInterface gl = (KinectGestures.GestureListenerInterface)monoScript;
					manager.gestureListeners.Add(monoScript);
				}
			}

		}
	}
	
}
