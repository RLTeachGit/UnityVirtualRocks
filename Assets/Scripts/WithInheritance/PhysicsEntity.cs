using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] //Allow only one of these
[RequireComponent(typeof(Collider2D))] //Make sure we have at least a collider
[RequireComponent(typeof(SpriteRenderer))] //And Sprite Renderer
public abstract class PhysicsEntity : MonoBehaviour {
    protected Rigidbody2D mRB; //Protected means children can see this
    protected Collider2D[] mColliders; //Can also be seen by children
    protected SpriteRenderer mSR; //Cached SpriteRenderer
    private Vector2 mVelocity = Vector2.zero;   //Internal Velocity variable

    private float mMaxSpeed = 5.0f; //Internal max Speed Value

    public bool mHasWarped;

    public Vector2 Velocity {
        get {
            return mVelocity;
        }
        set {
            if(value.magnitude > mMaxSpeed) { //Implement Speed clamp
                mVelocity = value.normalized * mMaxSpeed;   //Clamp to Max Speed
            } else {
                mVelocity = value;
            }
        }
    } //Velocity Properties
    public float MaxSpeed {
        get {
            return  mMaxSpeed;
        }
        set {
            mMaxSpeed = value;
            Velocity = Velocity;    //By calling this with itself this will clamp velocity if MaxSpeed was set after Velocity
        }
    } //MaxSpeed property


    private float mTimer;

    public float Timer {
        get {
            return mTimer;
        }
        set {
            if (value > 0) {
                mTimer = value;
            } else {
                mTimer = 0; //Only allow Positive values
            }
        }
    } //General purpose timer


    protected virtual void Start() { //By making Start() virtual we can override it in a derived class
        mRB = gameObject.AddComponent<Rigidbody2D>(); //Add a RigidBody in code
        mRB.isKinematic = true; //Turn off Unity Physics
        mColliders = GetComponents<Collider2D>(); //Get all the colliders on this object
        foreach(var tCollider in mColliders) { //Make sure all the colliders are triggers
            tCollider.isTrigger = true;
        }
        mSR = GetComponent<SpriteRenderer>(); //Get SpriteRenderer
        Debug.Assert(mColliders.Length > 0 && mSR != null, "Error: Required Components missing");
        mTimer = 0; //Reset timer
        mHasWarped = false;
    }

    private void Update() {  //Move using PhysicsEntity basic Physics
        UpdateMovement(); //Call Virtual function
        transform.position += (Vector3)mVelocity * Time.deltaTime;   //Need to upcast Velocity as transform.position is a Vector3
        WrapScreen();
        Timer -= Time.deltaTime;        //Update General purpose timer
    }

    protected virtual void UpdateMovement() {} //Empty function in base class as the default is just to keep moving as is

    void WrapScreen() {  //Almost the same as last week
        float tHeight = Camera.main.orthographicSize;   //Get Acreen size
        float tWidth = Camera.main.aspect * tHeight;

        if (transform.position.y > tHeight) {
            transform.position -= Vector3.up * 2 * tHeight;   //Offset to other side of screen
            mHasWarped = true;
        } else if (transform.position.y < -tHeight) {
            transform.position -= Vector3.down * 2 * tHeight;
            mHasWarped = true;
        }

        if (transform.position.x > tWidth) {
            transform.position -= Vector3.right * 2 * tWidth;
            mHasWarped = true;
        } else if (transform.position.x < -tWidth) {
            transform.position -= Vector3.left * 2 * tWidth;
            mHasWarped = true;
        }
    } //Almost the same as last week

    private void OnTriggerEnter2D(Collider2D vCollison) {
        PhysicsEntity tPhysicsEntity = vCollison.GetComponent<PhysicsEntity>(); //See of the the object we hit has a PhysicsEntity
        if(tPhysicsEntity!=null) {
            CollidedWith(tPhysicsEntity);
        } else {
            Debug.LogErrorFormat("Collision between {0} and non PhysicsEntity {1}", name, vCollison.name); //We his something not using Physics
        }
    }//We handle all the trigger stuff on the base class and then pass it up to the correct class to do its own

    protected virtual void CollidedWith(PhysicsEntity vOtherPhysicsEntity) {}  //Base class does nothing

    public  void   Show(bool vOn) {
        foreach (var mCollider in mColliders) { //Step through all the colliders
            mCollider.enabled = vOn;  //Turn them off
        }

        if (mSR != null) { //Show Sprite or not
            mSR.enabled = vOn;
        }
    }
}




