using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour {

    GameObject mLivesPanel;
      
	// Use this for initialization
	void Start () {
        HorizontalLayoutGroup tLayout = GetComponentInChildren<HorizontalLayoutGroup>();
        Debug.Assert(tLayout != null, "Cannot find layoutgroup");
        mLivesPanel = tLayout.gameObject; //Get Owning Panel

        StartCoroutine(UpdateLives());
    }

    IEnumerator UpdateLives() {
        for(; ; ) {
            int tDelta = GM.singleton.Lives - mLivesPanel.transform.childCount; //If the lives shown match the lives we have no action needed
            if (tDelta > 0) {
                while(tDelta-->0) {
                    Instantiate(GM.singleton.LifePrefab, mLivesPanel.transform, false); //Increase lives count
                }
            } else {
                tDelta = -tDelta;
                while(tDelta-->0) {
                    Destroy(mLivesPanel.transform.GetChild(0).gameObject); //Reduce lives count
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
