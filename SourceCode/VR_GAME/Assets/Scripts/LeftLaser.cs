using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//挂在左手controller下面
public class LeftLaser : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    private GameObject laser;
    private Transform laserTransform;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
