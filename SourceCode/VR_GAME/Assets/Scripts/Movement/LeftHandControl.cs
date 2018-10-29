using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandControl : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    public GameObject LeftHandCube;

    public GameObject highlightCube;
    public Transform Handpoint;
    public GameObject Walkhighlightpoint;
    public GameObject bodycontrol;
    public LayerMask teleportMask;

    bool isHitWalkable = false;


    // Use this for initialization
    float ori_scaleX;
    public float MyScaleSpeed = 10.0f;
    void Start () {
        //LeftHandCube.transform.position = trackedObj.transform.position + transform.forward* 0.5f;
        ori_scaleX = LeftHandCube.transform.localScale.x;

        highlightCube.SetActive(false);
        Walkhighlightpoint.SetActive(false);
        isHitWalkable = false;
     
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
    void Update () {

        MakeHighlight();
        if (Global.hand_state_left == Global.LENGTHEN_L)
        {
            Vector3 scale = LeftHandCube.transform.localScale;
            scale.x += MyScaleSpeed * Time.deltaTime;
            LeftHandCube.transform.localScale = scale;
        }
        else if (Global.hand_state_left == Global.SHORTEN_L)
        {
            Vector3 scale = LeftHandCube.transform.localScale;
            scale.x -= MyScaleSpeed * Time.deltaTime;
            if (scale.x <= ori_scaleX)
            {
                Global.hand_state_left = Global.NORMAL_L;
                scale.x = ori_scaleX;
            }
            LeftHandCube.transform.localScale = scale;
        }

        if (Global.state == Global.IDLE || Global.state == Global.CLIMB_R || Global.state == Global.SPIN_R) {
            // 处理输入
            // 空闲时、右手荡、右手滑、右手攀爬   左手允许动


            LeftHandCube.transform.rotation = trackedObj.transform.rotation;
            LeftHandCube.transform.Rotate(Vector3.up,-90);
        }

        if (Global.state == Global.IDLE && Global.hand_state_left == Global.NORMAL_L && Global.hand_state_right == Global.NORMAL_R)
        {
            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                /*
                print("left ButtonMask.Touchpad");
                Ray ray = new Ray(LeftHandCube.transform.position, LeftHandCube.transform.right * 100);          //定义一个射线对象,包含射线发射的位置transform.position，发射距离transform.forward*100；  
                RaycastHit hitInfo=new RaycastHit();                                 //定义一个RaycastHit变量用来保存被撞物体的信息；  
                if (Physics.Raycast(ray, out hitInfo, 100))         //如果碰撞到了物体，hitInfo里面就包含该物体的相关信息；  
                {
                    Debug.Log("hitinfo: " + hitInfo.collider.gameObject.name);
                    if (hitInfo.collider.gameObject.tag.Equals("Walkable")){
                        print("Left will move to a  Walkable");
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
                    print("left ButtonMask.Touchpad && isHitWalkable");
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
            print("left ButtonMask.Trigger");
            //左手伸缩：
            //  空闲时、右手荡时、右手攀爬时
            if (Global.state == Global.IDLE || Global.state == Global.CLIMB_R || Global.state == Global.SPIN_R || Global.state == Global.READY_SPIN_R)
            {
                
                if (Global.hand_state_left == Global.SHORTEN_L || Global.hand_state_left == Global.NORMAL_L)
                {
                    Global.hand_state_left = Global.LENGTHEN_L;
                }
                else
                {
                    Global.hand_state_left = Global.SHORTEN_L;
                }
            }
            // 如果左手在旋转，那就打断
            else if (Global.state == Global.SPIN_L || Global.state == Global.READY_SPIN_L || Global.state == Global.STROP_L)
            {
                // End this
                Global.state = Global.IDLE;
                Global.hand_state_left = Global.SHORTEN_L;
            }
            else if(Global.state == Global.CLIMB_L)
            {
                // End this
                Global.state = Global.IDLE;
                Global.hand_state_left = Global.SHORTEN_L;
                Global.touchThing = null;
            }
        }

        Vector3 veces = GetBoxColliderVertexPositions(LeftHandCube.GetComponent<BoxCollider>());
        Handpoint.transform.position = veces;
    }

    void MakeHighlight()
    {
     //   Debug.Log("MakeHighlight");

        Ray ray = new Ray(LeftHandCube.transform.position, LeftHandCube.transform.right * 100);          //定义一个射线对象,包含射线发射的位置transform.position，发射距离transform.forward*100；  
        Debug.DrawLine(LeftHandCube.transform.position, LeftHandCube.transform.position + LeftHandCube.transform.right * 100, Color.red);  //这个就是绘制出的射线了，包含发射位置，发射距离和射线的颜色；  
        RaycastHit hitInfo;                                 //定义一个RaycastHit变量用来保存被撞物体的信息；  
        if (Physics.Raycast(ray, out hitInfo, 100))         //如果碰撞到了物体，hitInfo里面就包含该物体的相关信息；  
        {
             // Debug.Log("hitinfo: " + hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject.tag.Equals("Touchable") || hitInfo.collider.gameObject.tag.Equals("Strop") || hitInfo.collider.gameObject.tag.Equals("Shortenable"))
            {
              //  Debug.Log("hit Touchable||Strop||Shortenable");

                highlightCube.transform.position = hitInfo.point;
                //highlightCube.GetComponent<MeshRenderer>().enabled = true;
                highlightCube.SetActive(true);
            } else if (hitInfo.collider.gameObject.tag.Equals("Walkable"))
            {
                if (Vector3.Distance(hitInfo.point, bodycontrol.transform.position) < 3f
                                && Mathf.Abs(hitInfo.point[1] - bodycontrol.transform.position[1]) < 0.5f)
                {
                    Debug.Log("hit Walkable");
                    Walkhighlightpoint.transform.position = hitInfo.point;
                    Walkhighlightpoint.SetActive(true);
                    isHitWalkable = true;
                    //Walkhighlightpoint.GetComponent<MeshRenderer>().enabled = true;
                }
               
            }
            else
            {
                highlightCube.SetActive(false);
              //  Walkhighlightpoint.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            highlightCube.SetActive(false);
            Walkhighlightpoint.SetActive(false);
            isHitWalkable = false;
           //Walkhighlightpoint.GetComponent<MeshRenderer>().enabled = false;
           //  highlightCube.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
