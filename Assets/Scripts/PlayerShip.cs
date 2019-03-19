using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : PhysicsEntity {  //We inherit from the player ship, which has itself been inherited from MonoBehaviour

    [SerializeField]
    private float AngularSpeed = 360.0f;

    [SerializeField]
    private float Speed = 1.0f;

    private  Healthbar mHealthbar;

    public  bool    IsDead { get; private set; } //Check if player Dead

    float tTimeSinceLastFire = 0;

    //This is the Player classes own version of Start()
    override protected void  Start() {
        base.Start();   //We call the base class start to start itself up
        mHealthbar = GetComponentInChildren<Healthbar>();
        IsDead = false;
    }

    protected override void UpdateMovement() {
        if (IsDead) return; //No more player control
        Movement();
        Shooting();
        DeathDebug();
    }

    void DeathDebug() {
        if (Input.GetButtonDown("Debug")) {
            TakeDamage(30);
        }
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
        if(tTimeSinceLastFire<=0) {
            if (Input.GetButton("Fire1")) {
                tTimeSinceLastFire = GM.singleton.StartFireRate; //Reset cooldown
                NewFire[] tFirePoint = GetComponentsInChildren<NewFire>();
                foreach (var tFP in tFirePoint) {
                    tFP.DoFire();
                }
            }
        } else {
            tTimeSinceLastFire -= Time.deltaTime;   //Fire cooldown
        }
    }

    public void TakeDamage(int vDamage) {
        mHealthbar.Health -= vDamage;
        if(mHealthbar.Health == 0) {
            IsDead = true;
            Show(false);
            mHealthbar.gameObject.SetActive(false);
            GetComponentInChildren<Explosion>().Explode();
        }
    }

    private void OnDestroy() {
        GM.singleton.PlayerDead();
    }
}



