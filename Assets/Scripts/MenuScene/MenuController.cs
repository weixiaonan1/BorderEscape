using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    private GameObject instruction;
    private GameObject menu;
    private MenuGestureListener gestureListener;

    //whether the introduction scene is active
    private bool IsInsDisplay = false;

    //interval between two input processing
    private const int cdCount = 50;
    private bool IsInCd = false;
    private int cd = cdCount;

    //0: kinect input; 1: keyboard input
    public static int mode;

    private void Awake()
    {
        instruction = GameObject.Find("Instruction");
        menu = GameObject.Find("Menu");
    }

    void Start()
    {
        instruction.SetActive(false);
        gestureListener = MenuGestureListener.Instance;
    }

    void Update()
    {
        if (gestureListener.IsZoomIn() && !IsInsDisplay)
        {
            if (IsInCd)
            {
                return;
            }
            menu.SetActive(false);
            IsInsDisplay = true;
            IsInCd = true;
            instruction.SetActive(true);
        }
        else if (gestureListener.IsZoomOut() && IsInsDisplay)
        {
            if (IsInCd)
            {
                return;
            }
            menu.SetActive(true);
            IsInsDisplay = false;
            IsInCd = true;

            instruction.SetActive(false);
        }
        else if (gestureListener.IsRun())
        {
            //load game scene and use kincet input;
            mode = 0;
            SceneManager.LoadScene("main", LoadSceneMode.Single);
            PlayerPrefs.SetInt("mode", mode);
        }

        if (IsInCd)
        {
            cd--;
        }
        if (cd < 0)
        {
            cd = cdCount;
            IsInCd = false;
        }
    }
    public void StartBtn()
    {
        //load game scene and use keyboard input;
        mode = 1;
        SceneManager.LoadScene("main", LoadSceneMode.Single);
        PlayerPrefs.SetInt("mode", mode);
    }

    public void InstructionBtn()
    {
        IsInsDisplay = true;
        menu.SetActive(false);
        instruction.SetActive(true);
    }

    public void ExitBtn()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void CloseBtn()
    {
        menu.SetActive(true);
        IsInsDisplay = false;
        instruction.SetActive(false);
    }
}