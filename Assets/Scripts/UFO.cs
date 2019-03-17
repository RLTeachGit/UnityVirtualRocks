using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour {


    FakePhysics mFakePhysics; //Cached FakePhysics code ref

    float mTimeOut = 5.0f;  //Countdown to next direction change

    public float Speed = 5.0f;  //Speed of UFO

    Rigidbody2D mRB2D; //Cached Variable for quick access

    // Use this for initialization
    void Start () {
        mFakePhysics = GetComponent<FakePhysics>(); //Add Fake Physics in code
        Debug.Assert(mFakePhysics != null, "Needs FakePhysics");    //Check its added ok

        mFakePhysics.MaxSpeed = 10.0f; //Set UFO Max Speed
        mFakePhysics.mVelocity = FakePhysics.RandomDirection() * Speed; //Set UFO Random direction

        mRB2D = gameObject.AddComponent<Rigidbody2D>(); //Add RB2D in code
        Debug.Assert(mRB2D != null, "Needs Rigidbody2D");    //Check its added ok
        mRB2D.isKinematic = true;   //Make it non physics in code, so triggers still work, but no forces act on it
    }

    // Update is called once per frame
    void Update () {
        UpdateDirection();  //Make random direction changes
    }

    //Make a random direction change every few seconds
    void UpdateDirection() {
        if(mTimeOut<=0) {
            mFakePhysics.mVelocity = FakePhysics.RandomDirection() * Speed; //Get Random direction an scale with speed
            mTimeOut = Random.Range(1,10); //Set new random timeout
            Fire[] tFirePoint = GetComponentsInChildren<Fire>();
            foreach (var tFP in tFirePoint) {
                tFP.DoFire();

            }
        } else {
            mTimeOut -=Time.deltaTime; //subtract the time since last update
        }
    }
}
