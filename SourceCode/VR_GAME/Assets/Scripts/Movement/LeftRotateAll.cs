using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRotateAll : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    Vector3 tempEuler = new Vector3();
    public GameObject control;
    // Use this for initialization
    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Global.state == Global.IDLE && Global.hand_state_left == Global.NORMAL_L && Global.hand_state_right == Global.NORMAL_R) //转动头、手、相机
        {
            if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                Debug.Log("Left ButtonMask.Grip");
                tempEuler = control.transform.eulerAngles;
                tempEuler.y = tempEuler.y - 60 * Time.deltaTime;
                //Debug.Log(" tempEuler.y: " + tempEuler.y);
                control.transform.eulerAngles = tempEuler;
            }
        }
    }
}
