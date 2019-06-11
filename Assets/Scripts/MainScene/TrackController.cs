using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour {
    //track prefabs
    public GameObject[] tracks;
    //initial speed
    public float initialSpeed;
    //accelerate rate per acceleratedNumber
    public float speedRate;
    //the number of completed tracks before accelerating
    public int acceleratedNumber;
    //track length
    public float length;


    //completed track
    [HideInInspector]
    public int count;
    //current speed
    [HideInInspector]
    public float currentSpeed;

    private void Awake() {
        count = 0;

        if(DataTransformer.initialSpeed == 0f) {
            currentSpeed = initialSpeed * Time.deltaTime;

            DataTransformer.initialSpeed = currentSpeed;
            DataTransformer.currentSpeed = currentSpeed;
        } else {
            currentSpeed = DataTransformer.initialSpeed;
        }
    }

	
	//complete one track
    public void RunOver() {
        count++;

        if (count % acceleratedNumber == 0) {
            currentSpeed = currentSpeed * speedRate;

            DataTransformer.currentSpeed = currentSpeed;
        }
    }

	//on pause button clicked
    public void Stop() {
        currentSpeed = 0;
    }

	//on continue button clicked
    public void Continue() {
        currentSpeed = DataTransformer.currentSpeed;
    }
}
