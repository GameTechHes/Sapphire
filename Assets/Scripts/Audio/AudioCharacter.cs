using Fusion;
using UnityEngine;

public class AudioCharacter : NetworkBehaviour
{
    private bool isLeftFoot;

    void Start()
    {
        isLeftFoot = true;
    }

    public void PlayWalkSFX(AnimationEvent animationEvent)
    {
        if(!Object.HasInputAuthority)
            return;
        if (animationEvent.animatorClipInfo.weight > 0.5)
        {
            int numberOfSound = isLeftFoot ? 3 : 4;
            isLeftFoot = !isLeftFoot;
            string soundName = "Walking_" + numberOfSound.ToString();
            FindObjectOfType<AudioManager>().Play(soundName);
        }
    }

    public void PlayRunningSFX(AnimationEvent animationEvent)
    {
        if(!Object.HasInputAuthority)
            return;
        if (animationEvent.animatorClipInfo.weight > 0.5)
        {
            int numberOfSound = isLeftFoot ? 2 : 3;
            isLeftFoot = !isLeftFoot;
            string soundName = "Running_" + numberOfSound.ToString();
            FindObjectOfType<AudioManager>().Play(soundName);
        }
    }

    public void PlayJumpingSFX()
    {
        if(!Object.HasInputAuthority)
            return;
        int numberOfSound = 5;
        int randomSoundID = Random.Range(1, numberOfSound);
        string soundName = "Running_" + randomSoundID.ToString();
        FindObjectOfType<AudioManager>().Play(soundName);
    }


    public void PlayJumpingLandSFX(AnimationEvent animationEvent)
    {
        if(!Object.HasInputAuthority)
            return;
        int numberOfSound = 5;
        int randomSoundID = Random.Range(1, numberOfSound);
        string soundName = "Running_" + randomSoundID.ToString();
        FindObjectOfType<AudioManager>().Play(soundName);
    }

    public void PlayHurtSFX()
    {
        if(!Object.HasInputAuthority)
            return;
        int numberOfSound = 4;
        int randomSoundID = Random.Range(1, numberOfSound);
        string soundName = "Hurt_" + randomSoundID.ToString();
        FindObjectOfType<AudioManager>().Play(soundName);
    }
}