using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class scrollbarvalue : MonoBehaviour {

    public Slider mslider;
    public GameObject hero;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        mslider.GetComponent<Slider>().value = hero.GetComponent<Fight>().lightEnergy;
        Debug.Log(hero.GetComponent<Fight>().lightEnergy);
    }

}
