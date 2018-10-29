using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    允许的状态切换：
        
        空闲     ->  其他任何状态
        左手荡   ->  右手荡   右手滑  右手攀爬   空闲
        右手荡   ->  左手荡   左手滑  左手攀爬   空闲
        左手滑   ->  空闲
        右手滑   ->  空闲
        左手攀爬 ->  右手荡   右手滑  右手攀爬   空闲
        右手攀爬 ->  左手荡   左手滑  左手攀爬   空闲

    左手WASD能动：
        空闲时、右手荡时、右手攀爬时
    
    左手伸缩：
        空闲时、右手荡时、右手攀爬时
     
     */

public class Global
{
    // define the states
    public const int IDLE = 0;

    // Left hand
    public const int SPIN_L = 1;
    public const int READY_SPIN_L = 2;
    public const int STROP_L = 5;
    public const int CLIMB_L = 7;

    // Right Hand
    public const int SPIN_R = 3;
    public const int READY_SPIN_R = 4;
    public const int STROP_R = 6;
    public const int CLIMB_R = 8;


    // hand scaleState
    public const int SHORTEN_L = 0;     // 左手变短
    public const int LENGTHEN_L = 1;    // 左手变长
    public const int NORMAL_L = 2;      // 左手正常长 or 人为静止
    public const int SHORTEN_R = 0;     // 右手变短
    public const int LENGTHEN_R = 1;    // 右手变长
    public const int NORMAL_R = 2;      // 右手正常长 or 人为静止


    //public const int 

    public static int state = IDLE;
    public static int hand_state_left = NORMAL_L;
    public static int hand_state_right = NORMAL_R;

    public static GameObject touchThing = null;

}

public class FollowControl : MonoBehaviour {
    Vector3 posOffset;
	// Use this for initialization
    public GameObject control;
	void Start ()
    {
     // posOffset = control.transform.position - transform.position;
        transform.position = control.transform.position;
        transform.rotation = control.transform.rotation;
   

    }
	
	// Update is called once per frame
	void Update () {
        // transform.position = control.transform.position- posOffset;
        transform.position = control.transform.position;
        transform.rotation = control.transform.rotation;

        //Debug.Log("hero state : " + Global.state);
        if (Global.state != Global.CLIMB_L || Global.state != Global.CLIMB_R)
        {
            Global.touchThing = null;
        }

        if (Global.state == Global.IDLE)
        {
            control.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
