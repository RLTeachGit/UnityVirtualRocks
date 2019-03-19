using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : BulletBase {

    GameObject mTarget;

    override protected void Start() {
        TimeToLive = -1; //Dont time out
        base.Start();   //We call the base class start to start itself up
        Speed = 7;
        Physics2D.queriesStartInColliders = false;
        mSR.color = Color.green;
    }

    protected override void UpdateMovement() {
        if(mTarget!=null) {
            PhysicsEntity tPhyEnt = mTarget.GetComponent<PhysicsEntity>();
            if(!tPhyEnt.mHasWarped) {
                Velocity = (mTarget.transform.position - transform.position).normalized * Speed;
                mSR.color = Color.red;
            } else {
                if(mHasWarped) { //Wait till we warp until position update
                    tPhyEnt.mHasWarped = false;
                    mHasWarped = false;
                }
            }
        } else {
            mSR.color = Color.green; //Lost tracking
            RaycastHit2D tHit = Physics2D.Raycast(transform.position, Velocity, Speed);
            if (tHit.collider != null) {
                PhysicsEntity tPhyEnt = tHit.collider.GetComponent<PhysicsEntity>();
                if (tPhyEnt is RockBase) {
                    mTarget = tHit.collider.gameObject; //Set Target
                    tPhyEnt.mHasWarped = false;
                    mHasWarped = false;
                }
            }
        }
    }

}
