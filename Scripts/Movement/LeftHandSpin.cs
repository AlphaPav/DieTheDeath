using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandSpin : MonoBehaviour {

    public GameObject control;
   


    // for ready spin state
    private float targetScaleX;
    private float oriScaleX;
    private float MyScaleSpeed = 10f;

    // for spin state
    float rotate_angle = 0.0f;                      // 旋转角度，用来还原
    Vector3 rotate_axis = new Vector3(0, 1, 0);     // 旋转的轴
    Vector3 rotate_body_angles;                     // 旋转前身体的角度
    Vector3 lastHeadPos = new Vector3();            // 上一刻头的位置
    Vector3 direction = new Vector3(0, 0, 0);       // 速度的切向方向
    Vector3 startSpinPos = new Vector3();           // 开始位置
    Vector3 pivot;                                  // 旋转时的坐标点
    float MySpeed = 0.0f;                           // 记录速度


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


    // Use this for initialization
    void Start () {
        oriScaleX = transform.localScale.x;
        lastHeadPos = control.transform.position;
      
    }
	
	// Update is called once per frame
	void Update () {
       
        if(Global.state == Global.READY_SPIN_L)
        {
            // 固定左手
            Global.hand_state_left = Global.NORMAL_L;
            control.GetComponent<Rigidbody>().isKinematic = true;
            if (transform.localScale[0] > targetScaleX)
            {
                // 缩小到目标大小
                float speed = 0;
                speed = Mathf.Pow(transform.localScale[0] / targetScaleX, 3) * MyScaleSpeed / 14;

                Vector3 scale = transform.localScale;
                scale[0] -= speed * Time.deltaTime;
                transform.localScale = scale;

                control.transform.Translate(transform.right * Time.deltaTime * speed, Space.World); //transform.right代表手的x轴坐标
            }
            else
            {
                // 更新到目标长度
                Vector3 scale = transform.localScale;
                scale[0] = targetScaleX;
                transform.localScale = scale;
                control.transform.Translate(transform.right * (transform.localScale[0] - targetScaleX));

                // record 
                startSpinPos = transform.position;
                pivot = GetBoxColliderVertexPositions(this.GetComponent<BoxCollider>());
                rotate_body_angles = control.transform.eulerAngles;
                Vector3 pointDir = pivot - startSpinPos;
                rotate_axis = new Vector3(-pointDir.z, 0, pointDir.x);

                // 开始旋转
                Global.state = Global.SPIN_L;
            }
        }
        else if(Global.state == Global.SPIN_L)
        {
            Debug.Log("正在旋转");

            // 当前速度
            // mgh = 1/2mv^2
            // v = sqrt(2 * g * h)

            if (control.transform.position[1] - startSpinPos[1] < 0)
            {
                MySpeed = Mathf.Sqrt(Mathf.Abs(2 * Vector3.Dot(Physics.gravity, control.transform.position - startSpinPos))) * 10;
            }
            else
            {
                MySpeed = 20;
            }

            control.transform.RotateAround(pivot, rotate_axis, MySpeed * Time.deltaTime);//第三个参数表示角度
            control.transform.Rotate(rotate_body_angles - control.transform.eulerAngles);//绕着自身坐标系旋转
            transform.RotateAround(control.transform.position, rotate_axis, MySpeed * Time.deltaTime);//第三个参数表示角度
            rotate_angle += MySpeed * Time.deltaTime;

            // 记录方向
            direction = (control.transform.position - lastHeadPos).normalized;

            if (control.transform.position.y - startSpinPos.y > 0.2f)
            {
                direction = new Vector3(0, 0, 0);
                // 旋转完成
                Global.state = Global.IDLE;
                Global.hand_state_left = Global.SHORTEN_L;
                control.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else
        {
            if (rotate_angle > 0)
            {
                // 回复到原来角度
                float diff_scale = transform.localScale.x - oriScaleX;
                float need_time_piece = diff_scale / MyScaleSpeed;
                if ((rotate_angle > 0.05) && (need_time_piece > Time.deltaTime))
                {
                    transform.RotateAround(control.transform.position, rotate_axis, -rotate_angle / need_time_piece * Time.deltaTime);
                    rotate_angle = rotate_angle - rotate_angle / need_time_piece * Time.deltaTime;
                }
                else
                {
                    transform.RotateAround(control.transform.position, rotate_axis, -rotate_angle);
                    rotate_angle = 0;
                }
            }

            // 赋予速度
            // 如果左手没有活动，恢复原状
            if ( !direction.Equals(new Vector3(0, 0, 0)))
            {
                control.GetComponent<Rigidbody>().velocity = 10f * direction;
                direction = new Vector3(0, 0, 0);
            }
            
        }

        lastHeadPos = control.transform.position;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Touchable"))
        {
            if(Global.state == Global.IDLE || Global.state == Global.SPIN_R || Global.state == Global.CLIMB_R || Global.state == Global.READY_SPIN_R)
            {
                // Update state
                Global.state = Global.READY_SPIN_L;
                Global.hand_state_left = Global.NORMAL_L;   // 静止
                Global.hand_state_right = Global.SHORTEN_R;  // 右手恢复

                // change state
                control.GetComponent<Rigidbody>().isKinematic = true;
                targetScaleX = transform.localScale[0] / 3 > oriScaleX ? transform.localScale[0] / 3 : oriScaleX;
            }
        }
    }

    
}
