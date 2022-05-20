using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioClip jumpSound, spawnSound, stompSound, upgradeSound, degradeSound, dieSound, bumpSound, kickSound, coinSound, smashSound, fireSound, bulletSound;
    static AudioSource audioSource;
    void Start()
    {
        jumpSound = Resources.Load<AudioClip>("Jump");
        upgradeSound = Resources.Load<AudioClip>("PowerUp");
        degradeSound = Resources.Load<AudioClip>("Degrade");
        dieSound = Resources.Load<AudioClip>("Die");
        stompSound = Resources.Load<AudioClip>("Stomp");
        bumpSound = Resources.Load<AudioClip>("Bump");
        spawnSound = Resources.Load<AudioClip>("Spawn");
        kickSound = Resources.Load<AudioClip>("Kick");
        coinSound = Resources.Load<AudioClip>("Coin");
        smashSound = Resources.Load<AudioClip>("Smash");
        fireSound = Resources.Load<AudioClip>("Fire");
        bulletSound = Resources.Load<AudioClip>("Bullet");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "jump":
                audioSource.PlayOneShot(jumpSound);
                break;
            case "upgrade":
                audioSource.PlayOneShot(upgradeSound);
                break;
            case "degrade":
                audioSource.PlayOneShot(degradeSound);
                break;
            case "die":
                audioSource.PlayOneShot(dieSound);
                break;
            case "stomp":
                audioSource.PlayOneShot(stompSound);
                break;
            case "bump":
                audioSource.PlayOneShot(bumpSound);
                break;
            case "spawn":
                audioSource.PlayOneShot(spawnSound);
                break;
            case "kick":
                audioSource.PlayOneShot(kickSound);
                break;
            case "coin":
                audioSource.PlayOneShot(coinSound);
                break;
            case "smash":
                audioSource.PlayOneShot(smashSound);
                break;
            case "fire":
                audioSource.PlayOneShot(fireSound);
                break;
            case "bullet":
                audioSource.PlayOneShot(bulletSound);
                break;
        }
    }
}
