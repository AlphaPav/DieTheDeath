using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandControl : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    public GameObject RightHandCube;

    public GameObject highlightCube;
    public Transform Handpoint;
    public GameObject Walkhighlightpoint;
    public GameObject bodycontrol;
    public LayerMask teleportMask;
    bool isHitWalkable = false;

    // Use this for initialization
    // Use this for initialization
    float ori_scaleX;
    public float MyScaleSpeed = 10.0f;
    void Start()
    {
       // RightHandCube.transform.position = trackedObj.transform.position + transform.forward * 0.5f;
        ori_scaleX = RightHandCube.transform.localScale.x;

        highlightCube.SetActive(false);
        Walkhighlightpoint.SetActive(false);

    }


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
    // Update is called once per frame
    void Update()
    {
        MakeHighlight();
        //处理伸缩
        if (Global.hand_state_right == Global.LENGTHEN_R) //变长
        {
            Vector3 scale = RightHandCube.transform.localScale;
            scale.x += MyScaleSpeed * Time.deltaTime;
            RightHandCube.transform.localScale = scale;
        }
        else if (Global.hand_state_right == Global.SHORTEN_R)//变短
        {
            Vector3 scale = RightHandCube.transform.localScale;
            scale.x -= MyScaleSpeed * Time.deltaTime;
            if (scale.x <= ori_scaleX)
            {
                Global.hand_state_right = Global.NORMAL_R;
                scale.x = ori_scaleX;
            }
            RightHandCube.transform.localScale = scale;
        }

        // 处理输入
        if (Global.state == Global.IDLE || Global.state == Global.CLIMB_L || Global.state == Global.SPIN_L) //右手可以改变方向的时刻：人物空闲，右手攀爬，右手荡
        {
            // 处理输入
           RightHandCube.transform.rotation = trackedObj.transform.rotation;
            RightHandCube.transform.Rotate(Vector3.up, -90);
        }

        if (Global.state == Global.IDLE && Global.hand_state_left == Global.NORMAL_L && Global.hand_state_right == Global.NORMAL_R)
        {
            
            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                /*//  print("right ButtonMask.Touchpad");
                  Ray ray = new Ray(RightHandCube.transform.position, RightHandCube.transform.right * 100);          //定义一个射线对象,包含射线发射的位置transform.position，发射距离transform.forward*100；  
                  RaycastHit hitInfo = new RaycastHit();                                 //定义一个RaycastHit变量用来保存被撞物体的信息；  
                  if (Physics.Raycast(ray, out hitInfo, 100))         //如果碰撞到了物体，hitInfo里面就包含该物体的相关信息；  
                  {
                     // Debug.Log("hitinfo: " + hitInfo.collider.gameObject.name);
                      if (hitInfo.collider.gameObject.tag.Equals("Walkable"))
                      {
                          print("right will move to a  Walkable");
                          if (Vector3.Distance(hitInfo.point, bodycontrol.transform.position) < 3f
                                  && Mathf.Abs(hitInfo.point[1] - bodycontrol.transform.position[1]) < 0.5f)
                          {
                              HeroMove.target = hitInfo.point;
                              HeroMove.target.y = bodycontrol.transform.position.y;
                              HeroMove.isMoveOver = false;
                              //2. 让角色移动到目标位置
                              HeroMove.source = bodycontrol.transform.position;

                          }
                      }

                  }*/
                if (isHitWalkable)
                {
                    print("Right ButtonMask.Touchpad && isHitWalkable");
                    HeroMove.target = Walkhighlightpoint.transform.position;
                    HeroMove.target.y = bodycontrol.transform.position.y;
                    HeroMove.isMoveOver = false;
                    HeroMove.source = bodycontrol.transform.position;
                }
            }
            
          
        }

        // handle Space
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {

            print("right ButtonMask.Trigger");
            if (Global.state == Global.SPIN_R || Global.state == Global.READY_SPIN_R || Global.state == Global.STROP_R)
            {
                // End this
                Global.state = Global.IDLE;
                Global.hand_state_right = Global.SHORTEN_R;
            }
            else if (Global.state == Global.IDLE || Global.state == Global.CLIMB_L || Global.state == Global.SPIN_L || Global.state == Global.READY_SPIN_L)
            {

                if (Global.hand_state_right == Global.SHORTEN_R || Global.hand_state_right == Global.NORMAL_R)
                {
                    Global.hand_state_right = Global.LENGTHEN_R;
                }
                else
                {
                    Global.hand_state_right = Global.SHORTEN_R;
                }
            }
            else if (Global.state == Global.CLIMB_R)
            {
                // End this
                Global.state = Global.IDLE;
                Global.hand_state_right = Global.SHORTEN_R;
                Global.touchThing = null;
            }
        }
        Vector3 veces = GetBoxColliderVertexPositions(RightHandCube.GetComponent<BoxCollider>());
        Handpoint.transform.position = veces;
    }

    void MakeHighlight()
    {


        Ray ray = new Ray(RightHandCube.transform.position, RightHandCube.transform.right * 100);          //定义一个射线对象,包含射线发射的位置transform.position，发射距离transform.forward*100；  
        Debug.DrawLine(RightHandCube.transform.position, RightHandCube.transform.position + RightHandCube.transform.right * 100, Color.red);  //这个就是绘制出的射线了，包含发射位置，发射距离和射线的颜色； 
        RaycastHit hitInfo;                                 //定义一个RaycastHit变量用来保存被撞物体的信息；  
        if (Physics.Raycast(ray, out hitInfo, 100))         //如果碰撞到了物体，hitInfo里面就包含该物体的相关信息；  
        {
            //  Debug.Log("hitinfo: " + hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject.tag.Equals("Touchable") || hitInfo.collider.gameObject.tag.Equals("Strop") || hitInfo.collider.gameObject.tag.Equals("Shortenable"))
            {
             //   Debug.Log("hit Touchable||Strop||Shortenable");

                highlightCube.transform.position = hitInfo.point;
                highlightCube.SetActive(true);
                // highlightCube.GetComponent<MeshRenderer>().enabled = true;
            }
            else if (hitInfo.collider.gameObject.tag.Equals("Walkable"))
            {
                if (Vector3.Distance(hitInfo.point, bodycontrol.transform.position) < 3f
                                && Mathf.Abs(hitInfo.point[1] - bodycontrol.transform.position[1]) < 0.5f)
                {
                  //  Debug.Log("hit Walkable");
                    Walkhighlightpoint.transform.position = hitInfo.point;
                    Walkhighlightpoint.SetActive(true);
                    isHitWalkable = true;

                }

            }
            else
            {
                highlightCube.SetActive(false);
           
            }

        }
        else
        {
            highlightCube.SetActive(false);
            Walkhighlightpoint.SetActive(false);
            isHitWalkable=false;
        }
    }

}
