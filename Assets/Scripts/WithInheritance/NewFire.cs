using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFire : MonoBehaviour {

    PhysicsEntity mParentPhysicsEntity; //Cached Parent PhysicsEntity

    // Use this for initialization
    void Start() {
        //We need some values from Parent's Physics, so grab them
        mParentPhysicsEntity = transform.root.gameObject.GetComponent<PhysicsEntity>(); //Get Parent FakePhysics
        Debug.Assert(mParentPhysicsEntity != null, "Could find Parent ParentPhysicsEntity");
    }


    public void DoFire() {
        Vector2 tFireDirection = transform.up; //Take fire position rotation as fire angle

        GameObject mBulletGO = Instantiate(GM.singleton.SplittingBullet, transform.position, Quaternion.identity);
        Debug.Assert(mBulletGO != null, "Could not create Bullet from prefab");

        PhysicsEntity tPhysicsEntity = mBulletGO.GetComponent<PhysicsEntity>(); //Fake Physics for Bullet
        Debug.Assert(tPhysicsEntity != null, "Could find Bullet PhysicsEntity Component");

        BulletBase tBulletBase = mBulletGO.GetComponent<BulletBase>();
        Debug.Assert(tBulletBase != null, "Could not find BulletBase Component");

        //Bullet velocity relative to ship
        tBulletBase.MaxSpeed += mParentPhysicsEntity.MaxSpeed;   //Allow playership speed + Bullet speed
        tBulletBase.Velocity += mParentPhysicsEntity.Velocity + tFireDirection * tBulletBase.Speed;    //Add ship velocity to bullet
    }
}
