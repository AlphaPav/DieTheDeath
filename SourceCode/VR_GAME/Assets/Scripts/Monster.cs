using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Monster : MonoBehaviour {
    float speed = 0.01f;//自己移动的速度
    float traceSpeed = 0.5f;//追逐主角的速度
    int clock = 0;
    int distance = 100;
    int moveState = 0;
    int isRotate = 0;
    float offset;
    public GameObject colliderArea;
    public GameObject hero;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (moveState == 0)
        {
            if (isRotate == 1)
            {
                transform.Rotate(0, 180, 0);
                isRotate = 0;
            }
            transform.Translate(-speed, 0, 0);
            clock++;
            if (clock == distance)
            {
                moveState = 1;
                clock = 0;
            }
        }
        else if (moveState == 1)//换一个方向移动
        {
            if (isRotate == 0)//旋转180度
            {
                transform.Rotate(0, 180, 0);
                isRotate = 1;
            }
            transform.Translate(-speed, 0, 0);
            clock++;
            if (clock == distance)
            {
                moveState = 0;
                clock = 0;
            }
        }
        if (colliderArea.GetComponent<ColliderArea>().isEnter == 1)//主角进入怪物活动范围
        {
            moveState = -1;
            transform.LookAt(hero.transform.position);
            transform.position = Vector3.Lerp(transform.position, hero.transform.position, Time.deltaTime * traceSpeed);//追逐
            offset = Vector3.Distance(transform.position, hero.transform.position);
            if (offset < 0.05f)
            {
                SceneManager.LoadScene("retry_die");
            }
            //Debug.Log(offset);
        }
        else if (moveState == -1 && colliderArea.GetComponent<ColliderArea>().isEnter == 0)//如果没追到主角已离开 就在原地开始运动
        {
            moveState = 0;
            isRotate = 0;
        }
    }
}
