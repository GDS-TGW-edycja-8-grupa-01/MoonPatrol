﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixerGroup output;
    public AudioClip engineSound;
    public AudioClip weaponShootUpSound;
    public AudioClip weaponShootDownSound;
    public AudioClip landSound;

    //public AudioSource audioSource;
    [Range(0.1f, 3.0f)]
    public float lowEnginePitch = 0.9f;
    [Range(0.1f, 3.0f)]
    public float hiEnginePitch = 1.5f;

    private PlayerBounds pb;
    private Vector2 minMaxBounds;
    private float pitchFactor;

    private AudioSource engineAudio;
    private AudioSource weaponUpAudio;
    private AudioSource weaponDownAudio;
    private AudioSource landAudio;

    // Start is called before the first frame update

    private void Awake()
    {
        engineAudio = NewAudio(engineSound, true, true, 0.2f);
        weaponUpAudio = NewAudio(weaponShootUpSound, false, false, 0.2f);
        weaponDownAudio = NewAudio(weaponShootDownSound, false, false, 0.3f);
        weaponDownAudio = NewAudio(weaponShootDownSound, false, false, 0.3f);
        landAudio = NewAudio(landSound, false, false, 0.2f);

    }

    void Start()
    {
        pb = GetComponent<PlayerBounds>();
        minMaxBounds = pb.GetBounds();
        engineAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        pitchFactor = Mathf.Abs(transform.position.x - minMaxBounds.x) / Mathf.Abs(minMaxBounds.x - minMaxBounds.y);
        engineAudio.pitch = Mathf.Lerp(lowEnginePitch, hiEnginePitch, Mathf.InverseLerp(0.0f, 1.0f, pitchFactor));
        //audioSource.pitch = pitchFactor;
        //Debug.Log("PITCHFACTOR : " + pitchFactor.ToString());
    }
    
    public AudioSource NewAudio(AudioClip clip, bool loop, bool playAwake, float volume)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = volume;
        newAudio.outputAudioMixerGroup = output;
        return newAudio;
    }

    public void WeaponShootUp()
    {
        weaponUpAudio.pitch = 1.0f + Random.Range(-0.3f, 0.3f);
        weaponUpAudio.PlayOneShot(weaponUpAudio.clip, 1.0f);
    }

    public void WeaponShootDown()
    {
        weaponDownAudio.Play();
    }

    public void EngineSoundStop()
    {
        engineAudio.Stop();
    }

    public void Land()
    {
        landAudio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        landAudio.PlayOneShot(landSound);
    }


}
