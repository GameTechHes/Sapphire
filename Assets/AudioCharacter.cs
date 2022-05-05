using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCharacter : MonoBehaviour
{ 
    public void PlayWalkSFX(AnimationEvent animationEvent){
        int numberOfSound = 5;
        if(animationEvent.animatorClipInfo.weight > 0.5) {
            int randomSoundID = Random.Range(1, numberOfSound);
            string soundName = "Walking_" +  randomSoundID.ToString();
            Debug.Log(soundName);
            FindObjectOfType<AudioManager>().Play(soundName);
        }
    }

    public void PlayRunningSFX(AnimationEvent animationEvent){
        int numberOfSound = 5;
        if(animationEvent.animatorClipInfo.weight > 0.5) {
            int randomSoundID = Random.Range(1, numberOfSound);
            string soundName = "Running_" +  randomSoundID.ToString();
            FindObjectOfType<AudioManager>().Play(soundName);
        }
    }

    public void PlayJumpingSFX(){
            int numberOfSound = 5;
            int randomSoundID = Random.Range(1, numberOfSound);
            string soundName = "Running_" +  randomSoundID.ToString();
            FindObjectOfType<AudioManager>().Play(soundName);
        }
    

    public void PlayJumpingLandSFX(AnimationEvent animationEvent){
            int numberOfSound = 5;
            int randomSoundID = Random.Range(1, numberOfSound);
            string soundName = "Running_" +  randomSoundID.ToString();
            FindObjectOfType<AudioManager>().Play(soundName);
        }

    public void PlayHurtSFX(){
            int numberOfSound = 4;
            int randomSoundID = Random.Range(1, numberOfSound);
            string soundName = "Hurt_" +  randomSoundID.ToString();
            FindObjectOfType<AudioManager>().Play(soundName);
    }
}

