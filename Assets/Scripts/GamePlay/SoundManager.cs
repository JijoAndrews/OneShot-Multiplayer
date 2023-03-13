using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static SoundManager instance;
    public GameObject soundPrefab;
    public AudioSource myAudioSource;

    public List<AudioClip> loopClips,impactAudioClips,hitLoops;

    public List<AudioSource> audioSourcesPresent;
    
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }



       

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StopBgm()
    {
        myAudioSource.Stop();
    }


    public void startSound(int clipId,float predelay,float timeNeed)
    {
        GameObject soundCopy = Instantiate(soundPrefab,transform);
        soundCopy.GetComponent<SoundSelfDestroy>().myManager = this;
        audioSourcesPresent.Add(soundCopy.GetComponent<AudioSource>());
        soundCopy.GetComponent<AudioSource>().clip = impactAudioClips[clipId];
        soundCopy.GetComponent<AudioSource>().PlayDelayed(predelay);
       // soundCopy.GetComponent<AudioSource>().Play();
        soundCopy.GetComponent<SoundSelfDestroy>().time = timeNeed;
    }



    public void RandomHit(int clipId, float predelay, float timeNeed)
    {
       
        GameObject soundCopy = Instantiate(soundPrefab, transform);
        soundCopy.GetComponent<SoundSelfDestroy>().myManager = this;
        audioSourcesPresent.Add(soundCopy.GetComponent<AudioSource>());
        soundCopy.GetComponent<AudioSource>().clip = hitLoops[Random.Range(0,hitLoops.Count-1)];
        soundCopy.GetComponent<AudioSource>().PlayDelayed(predelay);
        soundCopy.GetComponent<SoundSelfDestroy>().time = timeNeed;
    }


    public void ReverbSounds(bool status,float pitchVal)
    {
        foreach(AudioSource mySource in audioSourcesPresent)
        {
            if (status)
            {
                mySource.pitch = pitchVal;
            }
            else
            {
                mySource.pitch =1f;
            }
        }
    }
}
