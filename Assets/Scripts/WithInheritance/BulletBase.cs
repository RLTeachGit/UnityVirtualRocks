using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : PhysicsEntity {

    [SerializeField]
    protected float TimeToLive = 1.0f;

    public float Speed = 10.0f;

    // Use this for initialization
    override protected void Start() {
        base.Start();   //We call the base class start to start itself up
        if(TimeToLive>0)    Destroy(gameObject,TimeToLive);
    }
}
