using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    private int score;
    private int preScore;

    private Text scoreText;

    private void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    private void Start()
    {
        score = preScore = 0;
    }

    private void Update()
    {
        if (preScore != score)
        {
            preScore = score;

            scoreText.text = "Score：" + preScore;
        }
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue;
        GetComponent<AudioSource>().Play();
    }

    public void Reset()
    {
        score = 0;
    }
}
