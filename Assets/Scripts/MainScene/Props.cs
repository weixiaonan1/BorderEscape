using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour {
    public int scoreValue;
    private ScoreController scoreCtrl;

    private void Awake() {
        scoreCtrl = GameObject.Find("ScoreController").GetComponent<ScoreController>();
    }

    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            scoreCtrl.AddScore(scoreValue);
            
            Destroy(gameObject);
        }
    }
}
