using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    [SerializeField] //Makes non public variables show in IDE
    GameObject Bullet;  //Prefab Linked in IDE

    FakePhysics mParentFakePhysics; //Cached Parent FakePhysics

	// Use this for initialization
	void Start () {
        //We need some values from Parent's Physics, so grab them
        mParentFakePhysics = transform.root.gameObject.GetComponent<FakePhysics>(); //Get Parent FakePhysics
        Debug.Assert(mParentFakePhysics != null, "Could find Parent FakePhysics");

        Debug.Assert(Bullet != null, "Could find Bullet Prefab");
    }


    public void DoFire() {
        Vector2 tFireDirection = transform.up; //Take fire position rotation as fire angle

        GameObject mBulletGO = Instantiate(Bullet, transform.position, Quaternion.identity);
        Debug.Assert(mBulletGO != null, "Could not create Bullet from prefab");

        FakePhysics tBulletFakePhysics = mBulletGO.GetComponent<FakePhysics>(); //Fake Physics for Buller
        Debug.Assert(tBulletFakePhysics != null, "Could find Bullet FakePhysics Component");

        Bullet tBullet = mBulletGO.GetComponent<Bullet>();
        Debug.Assert(tBullet != null, "Could not find Bullet Component");

        //Bullet velocity relative to ship
        tBulletFakePhysics.mVelocity += mParentFakePhysics.mVelocity + tFireDirection * tBullet.Speed;    //Add ship velocity to bullet
        tBulletFakePhysics.MaxSpeed += mParentFakePhysics.MaxSpeed;   //Allow playership speed + Bullet speed
    }

}
