using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MoveShip : MonoBehaviour {

public float AngularSpeed = 360.0f;
public float Speed = 0.2f; //Slow down

    FakePhysics mFakePhysics; //Cached Variable for fast access
    Rigidbody2D mRB2D; //Cached Variable for quick access

    void Start() {
        mFakePhysics = GetComponent<FakePhysics>();  //Add FakePhysics Component in code
        Debug.Assert(mFakePhysics != null, "Needs FakePhysics");    //Check its added ok

        mFakePhysics.MaxSpeed = 10.0f; //Tell FakePhysics to clamp speed

        mRB2D = gameObject.AddComponent<Rigidbody2D>(); //Add RB2D in code
        Debug.Assert(mRB2D != null, "Needs Rigidbody2D");    //Check its added ok
        mRB2D.isKinematic = true;   //Make it non physics in code, so triggers still work, but no forces act on it




    }
    // Move players ship
    void Update () {
        float tHorizontal = Input.GetAxis("Horizontal"); //Get left/right
        float tVertical = Input.GetAxis("Vertical");    //Get up/down
        transform.Rotate(0, 0, -tHorizontal * AngularSpeed * Time.deltaTime); //Rotate ship 
        mFakePhysics.mVelocity += (Vector2)transform.up * Speed * tVertical * Time.deltaTime; //use FakePhysics, to give player velocity
                                                                                              // Update is called once per frame
        if (Input.GetButtonDown("Fire1")) {
            Fire[] tFirePoint = GetComponentsInChildren<Fire>();
            foreach(var tFP in tFirePoint) {
                tFP.DoFire();
            }
        }
    }
}
