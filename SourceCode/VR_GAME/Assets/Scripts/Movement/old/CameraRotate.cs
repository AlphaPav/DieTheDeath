using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {
    public GameObject head;

    float x = 0;
    float y = 0;
    Vector3 tempEuler = new Vector3();
    public static bool canRotHead = true;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (canRotHead) //转动头、相机
        {
            // x += Input.GetAxis("Mouse X");
            //y -= Input.GetAxis("Mouse Y");
            // y = ClampAngle(y, -180, 180);
            // x = ClampAngle(x, -180, 180);

            // head.transform.eulerAngles = new Vector3(y, x, 0);
            // transform.eulerAngles = new Vector3(y, x, 0);
            x= Input.GetAxis("Mouse X");
            y = -Input.GetAxis("Mouse Y");
            tempEuler = head.transform.eulerAngles;
            tempEuler.y = tempEuler.y + x;
            tempEuler.x = tempEuler.x +y;
            head.transform.eulerAngles = tempEuler;

            tempEuler = transform.eulerAngles;
            tempEuler.y = tempEuler.y + x;
            tempEuler.x = tempEuler.x + y;
            transform.eulerAngles = tempEuler;
        }
        else {
            
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
