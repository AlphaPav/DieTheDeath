using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderArea : MonoBehaviour {

    public int isEnter = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //进入怪物活动区域
        if (other.tag.Equals("Head"))
        {
            isEnter = 1;
            Debug.Log("collision enter");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //离开怪物活动区域
        if (other.tag.Equals("Head"))
        {
            isEnter = 0;
            Debug.Log("coliision exit");
        }
    }
}
