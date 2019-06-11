using UnityEngine;
using System.Collections;
using System;
//using Windows.Kinect;

public class GestureDetect : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    [Tooltip("GUI-Text to display gesture-listener messages and gesture information.")]
    public GUIText gestureInfo;

    // singleton instance of the class
    private static GestureDetect instance = null;

    // internal variables to track if progress message has been displayed
    private bool progressDisplayed;
    private float progressGestureTime;

    // whether the needed gesture has been detected or not
    private bool swipeLeft;
    private bool swipeRight;
    private bool stop;
    private bool wave;
    private bool jump;
    private bool squat;


    /// <summary>
    /// Gets the singleton GestureDetect instance.
    /// </summary>
    /// <value>The GestureDetect instance.</value>
    public static GestureDetect Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// Determines whether swipe left is detected.
    /// </summary>
    /// <returns><c>true</c> if swipe left is detected; otherwise, <c>false</c>.</returns>
    public bool IsSwipeLeft()
    {
        if (swipeLeft)
        {
            swipeLeft = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether swipe right is detected.
    /// </summary>
    /// <returns><c>true</c> if swipe right is detected; otherwise, <c>false</c>.</returns>
    public bool IsSwipeRight()
    {
        if (swipeRight)
        {
            swipeRight = false;
            return true;
        }

        return false;
    }

    public bool IsStop()
    {
        if (stop)
        {
            stop = false;
            return true;
        }

        return false;
    }

    public bool IsWave()
    {
        if (wave)
        {
            wave = false;
            return true;
        }

        return false;
    }

    public bool IsJump()
    {
        if (jump)
        {
            jump = false;
            return true;
        }

        return false;
    }

    public bool IsSquat()
    {
        if (squat)
        {
            squat = false;
            return true;
        }

        return false;
    }


    /// <summary>
    /// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    public void UserDetected(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;

        // detect these user specific gestures
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
        ////manager.DetectGesture(userId, KinectGestures.Gestures.Stop);  不知道Stop gesture是怎样的
        manager.DetectGesture(userId, KinectGestures.Gestures.Wave);
        manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
        manager.DetectGesture(userId, KinectGestures.Gestures.Squat);


        if (gestureInfo != null)
        {
            gestureInfo.GetComponent<GUIText>().text = "Swipe left, right to change the slides.Wave gesture are also detected";
        }
    }

    /// <summary>
    /// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    public void UserLost(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;

        if (gestureInfo != null)
        {
            gestureInfo.GetComponent<GUIText>().text = string.Empty;
        }
    }

    /// <summary>
    /// Invoked when a gesture is in progress.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="progress">Gesture progress [0..1]</param>
    /// <param name="joint">Joint type</param>
    /// <param name="screenPos">Normalized viewport position</param>
    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;

        if ((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                string sGestureText = string.Format("{0} - {1:F0}%", gesture, screenPos.z * 100f);
                gestureInfo.GetComponent<GUIText>().text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;
            }
        }
        else if ((gesture == KinectGestures.Gestures.Wheel || gesture == KinectGestures.Gestures.LeanLeft ||
                 gesture == KinectGestures.Gestures.LeanRight) && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                string sGestureText = string.Format("{0} - {1:F0} degrees", gesture, screenPos.z);
                gestureInfo.GetComponent<GUIText>().text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;
            }
        }
        else if (gesture == KinectGestures.Gestures.Run && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                string sGestureText = string.Format("{0} - progress: {1:F0}%", gesture, progress * 100);
                gestureInfo.GetComponent<GUIText>().text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;
            }
        }
    }

    /// <summary>
    /// Invoked if a gesture is completed.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="joint">Joint type</param>
    /// <param name="screenPos">Normalized viewport position</param>
    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return false;

        if (gestureInfo != null)
        {
            string sGestureText = gesture + " detected";
            gestureInfo.GetComponent<GUIText>().text = sGestureText;
        }

        if (gesture == KinectGestures.Gestures.SwipeLeft)
            swipeLeft = true;
        else if (gesture == KinectGestures.Gestures.SwipeRight)
            swipeRight = true;
        else if (gesture == KinectGestures.Gestures.Stop)
            stop = true;
        else if (gesture == KinectGestures.Gestures.Wave)
        {
            wave = true;
        }
        else if (gesture == KinectGestures.Gestures.Jump)
            jump = true;
        else if (gesture == KinectGestures.Gestures.Squat)
            squat = true;

        return true;
    }

    /// <summary>
    /// Invoked if a gesture is cancelled.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="joint">Joint type</param>
    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return false;

        if (progressDisplayed)
        {
            progressDisplayed = false;

            if (gestureInfo != null)
            {
                gestureInfo.GetComponent<GUIText>().text = String.Empty;
            }
        }

        return true;
    }


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (progressDisplayed && ((Time.realtimeSinceStartup - progressGestureTime) > 2f))
        {
            progressDisplayed = false;
            gestureInfo.GetComponent<GUIText>().text = String.Empty;

            Debug.Log("Forced progress to end.");
        }
    }

}

