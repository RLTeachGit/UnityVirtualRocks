using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplittingBullet : BulletBase {

    public float Angle = 20.0f;

    override protected void Start() {
        TimeToLive = -1; //Dont time out
        base.Start();   //We call the base class start to start itself up
        Speed = 7;
        Timer = 0.5f;
    }

    protected override void UpdateMovement() {
        if(Timer<=0) {
            GameObject tGO1 = Instantiate(GM.singleton.HomingBullet,transform.position,Quaternion.identity);
            BulletBase tBullet1 = tGO1.GetComponent<BulletBase>();
            tBullet1.Velocity= Quaternion.Euler(0, 0, -Angle) * Velocity;

            GameObject tGO2 = Instantiate(GM.singleton.HomingBullet, transform.position, Quaternion.identity);
            BulletBase tBullet2 = tGO2.GetComponent<BulletBase>();
            tBullet2.Velocity = Quaternion.Euler(0, 0, Angle) * Velocity;
            Timer = 0.5f;
            Destroy(gameObject,1.0f);
        }
    }
}
