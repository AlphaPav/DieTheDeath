using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightStrop : MonoBehaviour {

    // Use this for initialization

    public GameObject control;

    Vector3 stropMoveDir;
    float stropLen = 0;
    float tempStropLen = 0;
    float MyStropSpeed = 2f;
    public float MyScaleSpeed = 10f;
    Vector3 pivot;

    float oriScaleX;

    Vector3 GetBoxColliderVertexPositions(BoxCollider boxcollider)
    {
        var vertices = new Vector3[5];
        //下面4个点
        vertices[0] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, -boxcollider.size.y, boxcollider.size.z) * 0.5f);
        vertices[1] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, -boxcollider.size.y, -boxcollider.size.z) * 0.5f);
        //上面4个点
        vertices[2] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, boxcollider.size.y, boxcollider.size.z) * 0.5f);
        vertices[3] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, boxcollider.size.y, -boxcollider.size.z) * 0.5f);
        vertices[4] = (vertices[0] + vertices[1] + vertices[2] + vertices[3]) / 4;
        return vertices[4];
    }

    void Start()
    {
        oriScaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.state == Global.STROP_R)
        {
            if (transform.localScale.x != oriScaleX)//沿着杆下滑
            {
                control.transform.Translate(stropMoveDir * Time.deltaTime * MyStropSpeed, Space.World);
                tempStropLen += Time.deltaTime * MyStropSpeed;
                if (tempStropLen >= stropLen)
                {
                    control.GetComponent<Rigidbody>().isKinematic = false;
                    Global.state = Global.IDLE;
                }
            }
            else
            {
                control.GetComponent<Rigidbody>().isKinematic = true;
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

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Strop"))
        {
            if (Global.state == Global.IDLE || Global.state == Global.SPIN_L || Global.state == Global.READY_SPIN_L || Global.state == Global.CLIMB_L)
            {
                // Update state
                Global.state = Global.STROP_R;
                Global.hand_state_right = Global.NORMAL_R;   // short
                Global.hand_state_left = Global.SHORTEN_L;

                // change state
                control.GetComponent<Rigidbody>().isKinematic = true;
                stropMoveDir = other.transform.forward;

                pivot = GetBoxColliderVertexPositions(this.GetComponent<BoxCollider>());
                stropLen = (other.transform.position - pivot).magnitude + other.transform.localScale.z / 2;
            }
        }
    }
}
