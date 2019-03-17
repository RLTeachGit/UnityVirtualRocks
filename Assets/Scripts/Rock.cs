using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent] //Makes no sense to have more than one of these
public class Rock : MonoBehaviour {

    SpriteRenderer mSR; //For later use
    Collider2D[] mColliders; //Array of colliders, variable file scope

    float mAngle = 0.0f;
    public bool RandomVelocity = false;   //Public variables are shown in IDE

    FakePhysics mFakePhysics;   //Will be added in code

    Rigidbody2D mRB2D; //Cached Variable for quick access

    Explosion mExplosion; //Explosion component


    // Use this for initialization
    void Start () {


        mColliders = GetComponents<Collider2D>();   //Find colliders
        Debug.Assert(mColliders.Length > 0, "Collider(s) missing"); //Show error if not found

        mFakePhysics = GetComponent<FakePhysics>();  //Add required Fake physics in code
        Debug.Assert(mFakePhysics != null, "Needs FakePhysics");    //Check its added ok

        mExplosion = GetComponentInChildren<Explosion>();
        Debug.Assert(mExplosion != null, "No Explosion found in hierachy");

        foreach (var mCollider in mColliders) { //Step through all the colliders
            mCollider.isTrigger = true; //Use code to turn collider to triggers
        }

        mSR = GetComponent<SpriteRenderer>();   //Get SpriteRenderer
        Debug.Assert(mSR != null, "SpriteRenderer missing"); //Show error if not found

        mRB2D = gameObject.AddComponent<Rigidbody2D>(); //Add RB2D in code
        Debug.Assert(mRB2D != null, "Needs Rigidbody2D");    //Check its added ok
        mRB2D.isKinematic = true;   //Make it non physics in code, so triggers still work, but no forces act on it


        mAngle = Random.Range(-45.0f, 45.0f); //Set a random rotation rate

        if (RandomVelocity) {    //If IDE variable is set then give an initial random velocity
            mFakePhysics.mVelocity = FakePhysics.RandomDirection() * Random.Range(1.0f, 5.0f);
        }


    }

    void Update() {
        //rotate rock at mAngle per second, but using Time.deltaTime to scale it
        transform.Rotate(0, 0, mAngle * Time.deltaTime);
    }

    void    DoExplosion() {
        foreach (var mCollider in mColliders) { //Step through all the colliders
            mCollider.enabled = false;  //Turn Colliders off
        }

        if(mSR!=null) { //Don't show sprite anymore
            mSR.enabled = false;
        }
        mExplosion.Explode();   //Trigger Explosion
        GM.singleton.MyScore += 100;

    }


    private void OnTriggerEnter2D(Collider2D vCollision) { //Once when overlap starts
        Bullet tBullet = vCollision.GetComponent<Bullet>(); // If this returns null its not a bullet
        if(tBullet != null) {
            //Trigger explosion
            Destroy(tBullet.gameObject);    //Also kill bullet
            DoExplosion();
            GM.singleton.MyScore += 100;
        }
        Healthbar tHealthBar = vCollision.GetComponent<Healthbar>(); //Does object have a heathbar
        if(tHealthBar!=null) {
            tHealthBar.Health-=5; //Reduce health on collision
        }
    }
}
