using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Needed for UI

public class UpdateScore : MonoBehaviour {

    Text mScore;
	// Use this for initialization
	void Start () {
        mScore = GetComponent<Text>(); //Get reference to text on screen
	}
	
	// Update is called once per frame
	void Update () {
        mScore.text = string.Format("Score:{0}", GM.singleton.MyScore);
	}
}
