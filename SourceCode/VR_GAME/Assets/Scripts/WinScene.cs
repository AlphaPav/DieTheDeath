using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.tag.Equals("ExitCube"))
        {
           
            Application.Quit();

        }
        else if (other.tag.Equals("TriggerWin"))
        {
            Debug.Log("TriggerWin");
            SceneManager.LoadScene("Play");
        }
    }

}
