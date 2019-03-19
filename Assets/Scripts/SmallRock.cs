using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRock : RockBase {
    protected override void CollidedWith(PhysicsEntity vOtherPhysicsEntity) {
        if (vOtherPhysicsEntity is BulletBase) {
            Destroy(vOtherPhysicsEntity.gameObject);    //Also kill bullet
            DoExplosion();
            GM.singleton.BulletsHit++;
            GM.singleton.PlayerScore += GM.singleton.SmallRockScore;
        } else if (vOtherPhysicsEntity is PlayerShip) { //Now much easier to check what we hit
            PlayerShip tPlayer = (PlayerShip)vOtherPhysicsEntity; //Safe to cast as we know is a Playership
            DoExplosion();
            GM.singleton.PlayerScore += GM.singleton.SmallRockScore;
            tPlayer.TakeDamage(GM.singleton.SmallRockDamage);
        }
    }
}
