using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    public    Image GreenBar; //Link in IDE

    int mHealth=0;
    public  int Health {
        set {
            mHealth = Mathf.Clamp(value, 0, 100); //Make sure Health does not go out of range
            GreenBar.rectTransform.localScale = new Vector3(mHealth/100.0f, 1, 0); //Scale top bar to reveal bottom one
        }
        get {
            return  mHealth;
        }
    }
	// Use this for initialization
	void Start () {
        if (Debug.isDebugBuild) { //Only do this for debug
            CheckIDELinks();    //Make sure all the stuff which is needed is linked
        }
        Health = 100; //Max Health on start
    }
    //Check its all set up in IDE
    void CheckIDELinks() {
        Debug.Assert(GreenBar != null, "Please link Green Bar");
    }
}
