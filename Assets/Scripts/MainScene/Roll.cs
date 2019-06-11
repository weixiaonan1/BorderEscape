using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour {
	//track length
    private float length;
	//current track index
	private int trackIndex;
	//track instance
    private GameObject instance;
	//track controller
    private TrackController trackCtrl;
	//flag to create the first track
    private bool hasCreated;
	//track prefabs
    private GameObject[] tracks;

    private void Awake() {
        trackCtrl = GameObject.Find("TrackController").GetComponent<TrackController>();
    }
	
    private void Start() {
        tracks = trackCtrl.tracks;
        length = trackCtrl.length;
		trackIndex = 1;
		
        hasCreated = false;
    }

    private void Update() {
        transform.position = new Vector3(
            transform.position.x + trackCtrl.currentSpeed,
            transform.position.y,
            transform.position.z
        );
		//destroye one completed track
        if(transform.position.x > length) {
            Destroy(this.gameObject);

            trackCtrl.RunOver();
        }
		//create the first track
        if(!hasCreated && transform.position.x > 0) {
            hasCreated = true;

            CreateTrack();
        }
    }
	
	//generate infinite random track in track prefabs
    private void CreateTrack() {
        int index = Random.Range(0, tracks.Length);
		
		//ensure current track not to be instantiated again before completing it
		if(index == trackIndex){
			index++;
		}
		trackIndex = index;
		
        float xPos = transform.position.x - length;
        Vector3 pos = new Vector3(xPos, transform.position.y, transform.position.z);
        Instantiate(tracks[index], pos, Quaternion.identity);
    }	
}
