using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRotate : MonoBehaviour {
    public GameObject control;

    float x = 0;
    public float rotSpeed= 0.001f;

    public static bool canRotAll = true;
    Vector3 tempEuler = new Vector3();
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Global.state == Global.IDLE && Global.hand_state_left == Global.NORMAL_L && Global.hand_state_right == Global.NORMAL_R) //转动头、手、相机
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("KeyCode.LeftArrow");
                tempEuler = control.transform.eulerAngles;
                tempEuler.y = tempEuler.y - 60 * Time.deltaTime;
                //Debug.Log(" tempEuler.y: " + tempEuler.y);
                control.transform.eulerAngles = tempEuler;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Debug.Log("KeyCode.RightArrow");
                tempEuler = control.transform.eulerAngles;
                tempEuler.y = tempEuler.y + 60 * Time.deltaTime;
                //Debug.Log(" tempEuler.y: " + tempEuler.y);
                control.transform.eulerAngles = tempEuler;
            }
        }

    }
    //角度限制	
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

}
