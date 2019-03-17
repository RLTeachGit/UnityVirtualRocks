using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumRock : RockBase {
    protected override void CollidedWith(PhysicsEntity vOtherPhysicsEntity) {
        if (vOtherPhysicsEntity is BulletBase) {
            Destroy(vOtherPhysicsEntity.gameObject);    //Also kill bullet
            DoExplosion();
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            GM.singleton.MyScore += 200;
        } else if (vOtherPhysicsEntity is PlayerShip) { //Now much easier to check what we hit
            PlayerShip tPlayer = (PlayerShip)vOtherPhysicsEntity; //Safe to cast as we know is a Playership
            tPlayer.mHealthbar.Health -= 10;
            GM.singleton.MyScore += 10;
            DoExplosion();
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
        }
    }
}
