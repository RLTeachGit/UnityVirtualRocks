using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRock : RockBase {

    protected override void CollidedWith(PhysicsEntity vOtherPhysicsEntity) {
        if (vOtherPhysicsEntity is BulletBase) {
            Destroy(vOtherPhysicsEntity.gameObject);    //Also kill bullet
            DoExplosion();
            Instantiate(GM.singleton.RockPrefab[1], transform.position, Quaternion.identity); //Make 2 medium rocks
            Instantiate(GM.singleton.RockPrefab[1], transform.position, Quaternion.identity); //Make 2 medium rocks
            GM.singleton.BulletsHit++;
            GM.singleton.PlayerScore += GM.singleton.BigRockScore;
        } else if (vOtherPhysicsEntity is PlayerShip) { //Now much easier to check what we hit
            PlayerShip tPlayer = (PlayerShip)vOtherPhysicsEntity; //Safe to cast as we know is a Playership
            DoExplosion();
            Instantiate(GM.singleton.RockPrefab[1], transform.position, Quaternion.identity); //Make 2 medium rocks
            Instantiate(GM.singleton.RockPrefab[1], transform.position, Quaternion.identity); //Make 2 medium rocks
            GM.singleton.PlayerScore += GM.singleton.BigRockScore;
            tPlayer.TakeDamage(GM.singleton.BigRockDamage);
        }
    }
}
