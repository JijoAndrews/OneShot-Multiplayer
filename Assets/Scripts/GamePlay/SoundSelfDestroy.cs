using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSelfDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public SoundManager myManager;
    public float time;
    public bool enableToDie;
   
    void Start()
    {
        
    }


    public void timeToDie()
    {
        myManager.audioSourcesPresent.Remove(gameObject.GetComponent<AudioSource>());
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0 && !enableToDie)
        {
            enableToDie = true;
            Invoke("timeToDie", time);
        }
    }
}
