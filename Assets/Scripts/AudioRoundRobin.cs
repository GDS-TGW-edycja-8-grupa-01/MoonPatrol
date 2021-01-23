using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRoundRobin : MonoBehaviour
{
    public AudioClip[] sounds;
    //public UnityEngine.Audio.AudioMixerGroup output;
    public GameObject externalSoundPlayer;
    private AudioSource audioSource;
    private int lastPlayedIndex;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }

    void Start()
    {

        //lastPlayedIndex = sounds.Length + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoundRobinPlay(float volume)
    {
        int i = Random.Range(0, sounds.Length);
        GameObject soundPlayer = GameObject.Instantiate(externalSoundPlayer);
        audioSource = soundPlayer.GetComponent<AudioSource>();
        
       /* if(lastPlayedIndex == i)
        {
            AudioClip[] newSounds = new AudioClip[sounds.Length - 1];
            for (int j = 0; j < newSounds.Length - 1; j++)
            {
                if(j != i)
                {
                    newSounds[j] = sounds[j];
                }
            }
            int ii = Random.Range(0, newSounds.Length - 1);
            audioSource.PlayOneShot(sounds[ii], volume);
            if (ii >= i)
            {
                lastPlayedIndex = ii + 1;   
            }else
            {
                lastPlayedIndex = ii;   
            }
            return;
        }
        else
        {*/
            //zamienić w prefab
            audioSource.PlayOneShot(sounds[i], volume);
            //lastPlayedIndex = i;
            //return;
        //}
        
    }

    public void PlayFromArray(int index, float volume)
    {
        GameObject soundPlayer = GameObject.Instantiate(externalSoundPlayer);
        audioSource = soundPlayer.GetComponent<AudioSource>();
        audioSource.PlayOneShot(sounds[index], volume);
    }

    public void PlayFromArray(int index, float volume, float pitchVariation)
    {
        GameObject soundPlayer = GameObject.Instantiate(externalSoundPlayer);
        audioSource = soundPlayer.GetComponent<AudioSource>();
        audioSource.pitch = 1.0f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(sounds[index], volume);
        StartCoroutine(RestartPitch());
    }

    private IEnumerator RestartPitch()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        audioSource.pitch = 1.0f;
    }
}
