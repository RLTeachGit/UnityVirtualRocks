using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] //Only allow this once per GO
public class FakePhysics : MonoBehaviour {

    public float MaxSpeed = 5.0f;   //MaxSpeed can be set in IDE

    [HideInInspector]  //Don't show this in IDE
    public  Vector2 mVelocity = Vector2.zero;   //Initally we are still

    float mHeight;  //Cache Screen Size, assume its not going to change in game
    float mWidth;


	// Use this for initialization
	void Start () {
        mHeight = Camera.main.orthographicSize;   //This is slow as it seaches scene for camera
        mWidth = mHeight * Camera.main.aspect;
    }
    // Update is called once per frame
    void Update () {
        transform.position += (Vector3)mVelocity * Time.deltaTime; //Note cast to make Vector2 into Vector3
	}
    void LateUpdate() {

        SlowDown(); //Clamp Speed
        WrapScreen(); //Wrap world
    }

    //Uses properties of Vectors, the speed is the magnitude
    //By normalising this (making it lenght 1), you can them make it any lenght by multiplying
    void    SlowDown() {
        if (mVelocity.magnitude > MaxSpeed) { //Are we speeding?
            mVelocity = mVelocity.normalized * MaxSpeed; //Yes, then make a unit vector, then multiply by speed
        }
    }
    void    WrapScreen() {  //Similar to last weeks, but using cached variables

        if (transform.position.y > mHeight) {
            transform.position -= Vector3.up * 2 * mHeight;   //Offset to other side of screen
        } else if (transform.position.y < -mHeight) {
            transform.position -= Vector3.down * 2 * mHeight;
        }

        if (transform.position.x > mWidth) {
            transform.position -= Vector3.right * 2 * mWidth;
        } else if (transform.position.x < -mWidth) {
            transform.position -= Vector3.left * 2 * mWidth;
        }
    }

    //static is a special kind of method, which can be called
    //by just prefixing the class name
    //I put this here because I plan to use it in several places
    public  static  Vector2 RandomDirection() {
        //Ok, this is hard and I will explain in class
        //But read it back to front
        //Making a Up vector(0,1), which is already 1 long
        //Then rotating it by a random angle
        //the result is a normalised vector, in a random direction
        //to rotate with a Quaternion you multiply by it
        return Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.up;
    }

}
