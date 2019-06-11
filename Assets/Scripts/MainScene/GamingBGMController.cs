using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamingBGMController : MonoBehaviour {

    //音轨列表
    public AudioClip[] clips;

    //音源
    private AudioSource source;

    //当前音乐下标
    private int currClipIndex;

    //是否中断音频播放
    private bool m_play = false;

    //等待下一首歌
    private bool waitForNextClip = false;

    //已等待时间
    private float waitTime;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        source.clip = clips[0];
        currClipIndex = 0;
        waitForNextClip = false;
        m_play = false;
        waitTime = 0.0f;
        source.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_play)
        {
            source.Pause();
        }
        else {
            if (!source.isPlaying && !waitForNextClip) {
                int index = currClipIndex++;
                if (currClipIndex >= clips.Length)
                    currClipIndex = 0;
                source.clip = clips[currClipIndex];
                waitForNextClip = true;
            }
            if (waitForNextClip) {
                waitTime += Time.deltaTime;
                if (waitTime >= 10.0f)
                {
                    waitTime = 0.0f;
                    waitForNextClip = false;
                }
            }
            if (waitForNextClip)
                return;
            if(!source.isPlaying)
                source.Play();
        }
    }

    //终止BGM
    public void Stop() {
        m_play = true;
    }

    //继续音乐
    public void Resume() {
        m_play = false;
    }
}
