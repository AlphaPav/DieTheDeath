﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandShorten : MonoBehaviour {

    // Use this for initialization
    public GameObject control;
    public float MyScaleSpeed = 10f;

    float oriScaleX;
	void Start () {
        oriScaleX = transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (Global.state == Global.CLIMB_L)
        {
            Debug.Log(1);
            if (transform.localScale[0] > oriScaleX)//收缩到手臂原始长度
            {
                Vector3 scale = transform.localScale;
                scale[0] -= MyScaleSpeed * Time.deltaTime;
                transform.localScale = scale;
                control.transform.Translate(transform.right * Time.deltaTime * MyScaleSpeed, Space.World); //transform.right代表手的x轴坐标
            }
            else
            {
                Vector3 scale = transform.localScale;
                scale[0] = oriScaleX;
                transform.localScale = scale;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Shortenable"))
        {
            if (Global.state == Global.IDLE || Global.state == Global.CLIMB_R || Global.state == Global.READY_SPIN_R || Global.state == Global.SPIN_R)
            {
                if(Global.touchThing != other.gameObject)
                {
                    // Update state
                    Global.state = Global.CLIMB_L;
                    Global.hand_state_left = Global.NORMAL_L;   // short
                    Global.hand_state_right = Global.SHORTEN_R;

                    Global.touchThing = other.gameObject;
                    control.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
}
