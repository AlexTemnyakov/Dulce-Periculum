using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GoblinVoice : MonoBehaviour
{
    //public AudioClip[]   SOUNDS;
    public AudioClip     NORMAL_SOUND;
    public float         NORMAL_SOUND_INTERVAL;
    public AudioClip     ATTACK_SOUND;
    public float         ATTACK_SOUND_INTERVAL;

    private GoblinHealth health         = null;
    private AudioSource  audioSource    = null;
    private float        timeAfterSound = 0;
    private int          currentSound   = 0;

    void Start()
    {
        health         = GetComponent<GoblinHealth>();
        audioSource    = GetComponent<AudioSource>();
        timeAfterSound = Random.Range(0, NORMAL_SOUND_INTERVAL);
    }

    void Update()
    {
        timeAfterSound += Time.deltaTime;
    }

    public void MakeNormalSound()
    {
        if (timeAfterSound >= NORMAL_SOUND_INTERVAL)
        {
            audioSource.PlayOneShot(NORMAL_SOUND);
            timeAfterSound = 0;
        }
    }

    public void MakeAttackSound()
    {
        if (timeAfterSound >= ATTACK_SOUND_INTERVAL)
        {
            audioSource.PlayOneShot(ATTACK_SOUND);
            timeAfterSound = 0;
        }
    }
}
