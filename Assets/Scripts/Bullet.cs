using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float Speed = 10.0f;
    public float TimeToLive = 10.0f;

    FakePhysics mFakePhysics; //Cahed copy of FakePhysics
    Collider2D mCollider2D; //Cached copy of Collider
                            // Use this for initialization
    void Start() {

        mFakePhysics = GetComponent<FakePhysics>();  //Add FakePhysics Component in code
        Debug.Assert(mFakePhysics != null, "Needs FakePhysics");    //Check its added ok

        mCollider2D = GetComponent<Collider2D>();
        Debug.Assert(mCollider2D != null, "Bullet needs a collider");
        mCollider2D.isTrigger = true; //set Trigger in code so we don't forget

        Destroy(gameObject, TimeToLive);      //Time to live
    }
}
