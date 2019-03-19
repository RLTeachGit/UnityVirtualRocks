using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    Text mText;

    public bool Over {
        get {
            return mText.enabled;
        }
        set {
            mText.enabled=value;
        }
    }

    private void Awake() {
        mText = GetComponent<Text>();
        Over = false;
    }
}
