using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    ParticleSystem mParticleSystem; //Cached Reference to ParticleSystem
    AudioSource mAudioSource;   //Cached Reference to AudioSource

    // Use this for initialization
    void Start () {

        mParticleSystem = GetComponent<ParticleSystem>();   //Get Reference to ParticleSystem
        Debug.Assert(mParticleSystem != null, "No ParticleSystem component");

        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(mAudioSource != null, "No AudioSource component");
    }

    public  void    Explode() {
        mParticleSystem.Play();
        mAudioSource.Play();
    }

    void OnDestroy() {
        Debug.Log("{0} Destroyed");
        Destroy(transform.root.gameObject); //Destroy parent as well
    }
}
