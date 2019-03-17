using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : PhysicsEntity {  //We inherit from the player ship, which has itself been inherited from MonoBehaviour

    [SerializeField]
    private float AngularSpeed = 360.0f;

    [SerializeField]
    private float Speed = 1.0f;

    public  Healthbar mHealthbar;

    //This is the Player classes own version of Start()
    override protected void  Start() {
        base.Start();   //We call the base class start to start itself up
        mHealthbar = GetComponentInChildren<Healthbar>();
    }

    protected override void UpdateMovement() {
        Movement();
        Shooting();
    }

    //Handle movement
    void    Movement() {
        float tHorizontal = Input.GetAxis("Horizontal"); //Get left/right
        float tVertical = Input.GetAxis("Vertical");    //Get up/down
        transform.Rotate(0, 0, -tHorizontal * AngularSpeed * Time.deltaTime); //Rotate ship 
        Velocity += (Vector2)transform.up * Speed * tVertical * Time.deltaTime; //use FakePhysics, to give player velocity
    }

    //Handle firing
    void Shooting() {
        if (Input.GetButtonDown("Fire1")) {
            NewFire[] tFirePoint = GetComponentsInChildren<NewFire>();
            foreach (var tFP in tFirePoint) {
                tFP.DoFire();
            }
        }
    }
}



