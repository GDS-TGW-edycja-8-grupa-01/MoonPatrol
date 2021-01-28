using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicScript : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    [Range(0.0f, 1.0f)]
    public float volume = 0.635f;
    public AudioMixerGroup output;
    [Range(0, 10)]
    public int crossfadeLength;

    private AudioSource audioSourceM;
    private AudioSource audioSourceG;
    
    // Start is called before the first frame update
    private void Awake()
    {
        audioSourceM = NewAudio(menuMusic, true, true, volume);
        audioSourceG = NewAudio(gameMusic, true, false, 0.0f);
        audioSourceM.Play();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchMusic(bool menu)
    {
        if(!menu && audioSourceM.isPlaying)
        {
            StartCoroutine(StopMusic(audioSourceM, crossfadeLength));
            StartCoroutine(PlayMusic(audioSourceG, crossfadeLength));
        }
        else if(menu && !audioSourceM.isPlaying)
        {
            StartCoroutine(StopMusic(audioSourceG, crossfadeLength));
            StartCoroutine(PlayMusic(audioSourceM, crossfadeLength));
        }
    }

    public IEnumerator StopMusic(AudioSource audioSource, int frames)
    {
        if (frames != 0)
        {
            while (audioSource.isPlaying)
            {
                yield return new WaitForEndOfFrame();
                audioSource.volume -= volume / frames;
                Mathf.Clamp(audioSource.volume, 0.0f, 1.0f);
                if (audioSource.volume <= 0.0f) audioSource.Stop();
            }
        }
        else
        {
            audioSource.Stop();
            audioSource.volume = 0;
        }
    }

    private IEnumerator PlayMusic(AudioSource audioSource, int frames)
    {
        if (frames != 0)
        {
            audioSource.volume = 0.0f;
            audioSource.Play();
            while (audioSource.volume < volume)
            {
                yield return new WaitForEndOfFrame();
                audioSource.volume += volume / frames;
                Mathf.Clamp(audioSource.volume, 0.0f, volume);
            }
        }
        else
        {
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    private AudioSource NewAudio(AudioClip clip, bool loop, bool playAwake, float volume)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = volume;
        newAudio.outputAudioMixerGroup = output;
        return newAudio;
    }
}
