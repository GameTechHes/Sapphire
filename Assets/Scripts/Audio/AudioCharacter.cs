using UnityEngine;

public class AudioCharacter : MonoBehaviour
{
    private bool isLeftFoot;

    void Start(){
        isLeftFoot = true;
    }
    public void PlayWalkSFX(AnimationEvent animationEvent){

        if(animationEvent.animatorClipInfo.weight > 0.5) {
            int numberOfSound = isLeftFoot ? 3 : 4;
            isLeftFoot = !isLeftFoot;            
            string soundName = "Walking_" +  numberOfSound.ToString();
            FindObjectOfType<AudioManager>().Play(soundName);
        }
    }

    public void PlayRunningSFX(AnimationEvent animationEvent){
        if(animationEvent.animatorClipInfo.weight > 0.5) {
            int numberOfSound = isLeftFoot ? 2 : 3;
            isLeftFoot = !isLeftFoot;            
            string soundName = "Running_" +  numberOfSound.ToString();
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

