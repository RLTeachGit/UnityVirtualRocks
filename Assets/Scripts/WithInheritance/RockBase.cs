using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RockBase : PhysicsEntity {

    protected float mAngle = 0.0f;
    private Explosion mExplosion; //Explosion component

    private static  int mRockCount=0; //Only expose via Getter

    public static int   RockCount {
        get {
            return mRockCount;
        }
    } //Static Getter, same for all rocks


    private void Awake() {
        mRockCount++;   //When new rock comes into existance
    }

    override protected void Start() {
        base.Start();   //We call the base class start to start itself up
        Velocity = GM.RandomDirection() * 5.0f;

        mExplosion = GetComponentInChildren<Explosion>();
        Debug.Assert(mExplosion != null, "No Explosion found in hierachy");
    }

    protected override void UpdateMovement() {
        transform.Rotate(0, 0, mAngle * Time.deltaTime);
    }
    private void OnDestroy() {
        mRockCount--;
    }

    protected void DoExplosion() {
        Show(false);    //Turn off the sprite/colliders
        mExplosion.Explode();   //Trigger Explosion
    }
}
