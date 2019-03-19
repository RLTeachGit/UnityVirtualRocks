using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    ParticleSystem mParticleSystem; //Cached Reference to ParticleSystem
    AudioSource mAudioSource;   //Cached Reference to AudioSource

    // Use this for initialization
    void Start() {

        mParticleSystem = GetComponent<ParticleSystem>();   //Get Reference to ParticleSystem
        Debug.Assert(mParticleSystem != null, "No ParticleSystem component");

        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(mAudioSource != null, "No AudioSource component");
    }

    public void Explode() {
        mParticleSystem.Play(); //Show Explosion
        mAudioSource.Play(); //Sound
        StartCoroutine(CheckEndOfPlay());
    }

    IEnumerator CheckEndOfPlay() {
        while (mParticleSystem.isPlaying || mAudioSource.isPlaying) {
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(transform.root.gameObject);
    }
}
