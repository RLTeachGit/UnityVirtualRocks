using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumRock : RockBase {
    protected override void CollidedWith(PhysicsEntity vOtherPhysicsEntity) {
        if (vOtherPhysicsEntity is BulletBase) {
            Destroy(vOtherPhysicsEntity.gameObject);    //Also kill bullet
            GM.singleton.BulletsHit++;
            DoExplosion();
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            GM.singleton.PlayerScore += GM.singleton.MediumRockScore;
        } else if (vOtherPhysicsEntity is PlayerShip) { //Now much easier to check what we hit
            PlayerShip tPlayer = (PlayerShip)vOtherPhysicsEntity; //Safe to cast as we know is a Playership
            tPlayer.TakeDamage(10);
            GM.singleton.PlayerScore += 10;
            DoExplosion();
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            Instantiate(GM.singleton.RockPrefab[2], transform.position, Quaternion.identity); //Make 3 small rocks
            GM.singleton.PlayerScore += GM.singleton.MediumRockScore;
            tPlayer.TakeDamage(GM.singleton.MediumRockDamage);
        }
    }
}
