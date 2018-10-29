using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tiggerdie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.tag.Equals("TriggerDie"))
        {
            Debug.Log("TriggerDie");
            SceneManager.LoadScene("retry_die");

        }
        else if (other.tag.Equals("TriggerWin"))
        {
            Debug.Log("TriggerWin");
            SceneManager.LoadScene("retry_win");
        }
    }
}
