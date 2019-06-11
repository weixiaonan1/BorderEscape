using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GameObject pausedMenu;
    private GameObject restartMenu;

    private TrackController trackCtrl;

    private PlayerGestureListener gestureListener;
    private bool isPause = false;
    private bool isRestart = false;
    private void Awake()
    {
        trackCtrl = GameObject.Find("TrackController").GetComponent<TrackController>();

        pausedMenu = GameObject.Find("PausedMenu");
        pausedMenu.SetActive(false);

        restartMenu = GameObject.Find("RestartMenu");
        restartMenu.SetActive(false);
    }

    void Start()
    {
        // get the gestures listener
        gestureListener = PlayerGestureListener.Instance;
    }

    void Update()
    {
        if (!gestureListener)
            return;

        if (gestureListener.IsWave())
        {           
            if(isRestart)
            {
                Restart();
            }

        }
        if (gestureListener.IsSwipeUp())
        {
            if (!isPause)
            {
                Pause();

            }
            else
            {
                Continue();

            }
        }
    }

    //游戏结束调用的函数
    public void Gameover()
    {
        Time.timeScale = 0;

        trackCtrl.Stop();

        restartMenu.SetActive(true);
        isRestart = true;
    }

    //游戏暂停调研的函数
    public void Pause()
    {
        isPause = true;
        Time.timeScale = 0;

        trackCtrl.Stop();

        pausedMenu.SetActive(true);
    }

    //游戏继续调研的函数
    public void Continue()
    {
        isPause = false;
        Time.timeScale = 1;

        trackCtrl.Continue();

        pausedMenu.SetActive(false);
    }

    //游戏重新开始调用的游戏
    public void Restart() {
        isRestart = false;

        SceneManager.LoadScene("main", LoadSceneMode.Single);

        Time.timeScale = 1;
    }
    //退出游戏
    public void Exit() {
        Application.Quit();
    }
}
