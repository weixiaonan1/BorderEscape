using UnityEngine;
using System.Collections;
using System;
//using Windows.Kinect;

public class MenuGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{

    // singleton instance of the class
    private static MenuGestureListener instance = null;

    // internal variables to track if progress message has been displayed
    private bool progressDisplayed;
    private float progressGestureTime;

    // whether the needed gesture has been detected or not
    private bool run;
    private bool zoomIn;
    private bool zoomOut;

    /// <summary>
    /// Gets the singleton MenuGestureListener instance.
    /// </summary>
    /// <value>The MenuGestureListener instance.</value>
    public static MenuGestureListener Instance
    {
        get
        {
            return instance;
        }
    }

    public bool IsRun()
    {
        if (run)
        {
            run = false;
            return true;
        }
        return false;
    }

    public bool IsZoomIn()
    {
        if (zoomIn)
        {
            zoomIn = false;
            return true;
        }
        return false;

    }

    public bool IsZoomOut()
    {
        if (zoomOut)
        {
            zoomOut = false;
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

        manager.DetectGesture(userId, KinectGestures.Gestures.Run);
        manager.DetectGesture(userId, KinectGestures.Gestures.ZoomIn);
        manager.DetectGesture(userId, KinectGestures.Gestures.ZoomOut);

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

        if (gesture == KinectGestures.Gestures.ZoomIn && progress > 0.5f)
        {
            zoomIn = true;
            progressDisplayed = true;
            progressGestureTime = Time.realtimeSinceStartup;
        }
        else if (gesture == KinectGestures.Gestures.ZoomOut && progress > 0.5f)
        {
            zoomOut = true;
            progressDisplayed = true;
            progressGestureTime = Time.realtimeSinceStartup;
        }
        else if (gesture == KinectGestures.Gestures.Run && progress > 0.5f)
        {
            run = true;
            progressDisplayed = true;
            progressGestureTime = Time.realtimeSinceStartup;
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

            Debug.Log("Forced progress to end.");
        }
    }

}
