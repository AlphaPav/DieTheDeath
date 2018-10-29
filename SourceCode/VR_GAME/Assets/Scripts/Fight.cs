using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    GameObject monster;
    GameObject heroLight;
    Vector3 dir;
    int isPushed = 0;
    float speed = 5f;
    int clock = 0;
    int showLight = 0;
    public float lightEnergy = 0;
    int lightflag = 0;
    float hSbarValue;
    //public GameObject mslider;

    // Use this for initialization
    void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Ghost");
        heroLight = GameObject.FindGameObjectWithTag("HeroLight");
        heroLight.GetComponent<Light>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            Debug.Log("SteamVR_Controller.ButtonMask.ApplicationMenu");
            //showLight = 1;
            lightflag = 1;
           
            if (Vector3.Distance(transform.position, monster.transform.position) < 2f && lightEnergy > 5f)//怪物和主角距离小于2 可以被推开
            {
                dir = transform.position - monster.transform.position;
                isPushed = 1;
            }
        }
        if (isPushed == 1)//怪物可以被推开
        {
            monster.transform.position -= dir * speed;//朝反方向运动 但是还没有设置什么时候停止...
        }
        if (lightflag == 1)
        {
            if (lightEnergy > 5f)
            {
                lightEnergy -= 5f;
                showLight = 1;
                lightflag = 0;
            }
            lightflag = 0;
        }
        if (lightEnergy < 100f)
        {
            lightEnergy += 0.05f;
        }
        if (lightEnergy <= 0f)
        {
            lightEnergy = 0f;
        }
        if (showLight == 1)
        {
            if (clock < 10)
            {
                heroLight.GetComponent<Light>().enabled = true;
                heroLight.GetComponent<Light>().range += 0.02f;
                clock++;
            }
            else
            {
                heroLight.GetComponent<Light>().enabled = false;
                heroLight.GetComponent<Light>().range = 0.6f;
                clock = 0;
                showLight = 0;
            }
        }
       // mslider.value = lightEnergy;
    }
    /*
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            border = new RectOffset(10, 10, 10, 10),
            fontSize = 20,
            fontStyle = FontStyle.Bold,
        };
        style.normal.textColor = new Color(120/255f, 120/255f, 200/255f);
        //GUI.Label(new Rect(10, 10, 200, 50), "light energy:" + lightEnergy.ToString("f2"), style);
        //GUI.Label(new Rect(30, 180, 100, 200), "滑动条");
        hSbarValue = GUI.HorizontalScrollbar(new Rect(30, 320, 100, 30), 5, 1.0f, 0.0f, 10.0f);

    }*/

}
