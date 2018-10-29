using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger1 : MonoBehaviour
{
    public GameObject control;

    // Vector3 offset = new Vector3(0, 6.346f, 0);
    Vector3 offset = new Vector3(0, 6.6f, 0);
    float speed = 2.0f;
    bool isMove = false;
    Vector3 tatPos;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (isMove)
        {
            if (Mathf.Abs(tatPos.y - transform.position.y)>0.1)
            {
                HeroRotate.canRotAll = false;
                HeroMove.canMove = false;
                control.GetComponent<Rigidbody>().isKinematic = true;
                control.transform.Translate((tatPos - transform.position) , Space.World);
            }
            else
            {
                Debug.Log("else");
                isMove = false;
                HeroMove.canMove = true;
                HeroRotate.canRotAll = true;
                control.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
       

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.tag.Equals("Trigger1"))
        {
            Debug.Log("OnTriggerEnterTrigger1");
            isMove = true;
            tatPos = control.transform.position + offset;
        }
    }
}
