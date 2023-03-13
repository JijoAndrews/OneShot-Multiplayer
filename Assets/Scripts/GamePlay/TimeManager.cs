using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float slowdownFactor=0.05f;
    public float slowdownLength =2f;
    public static TimeManager instance;

    void Start()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
       // SoundManager.instance.ReverbSounds(true);
        StartCoroutine(RevereSlowMotion());
    }


    public void UndoSlowmotion()
    {
       // SoundManager.instance.ReverbSounds(false);
        Time.timeScale = 1f;
    }


    IEnumerator RevereSlowMotion()
    {
        yield return new WaitForSeconds(0.05f);
        UndoSlowmotion();
    }

    public void TimeSlowForever()
    {
       // Time.timeScale = slowdownFactor;
       // Time.fixedDeltaTime = Time.timeScale * 0.02f;

        StartCoroutine(WaitAndStopForever());
    }



    IEnumerator WaitAndStopForever()
    {
        yield return new WaitForSeconds(1.4f);
        Time.timeScale = 0.001f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }


   public void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }


    public void DoVRSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        // SoundManager.instance.ReverbSounds(true);
        StartCoroutine(RevereVRSlowMotion());
    }


    IEnumerator RevereVRSlowMotion()
    {
        yield return new WaitForSeconds(0.05f);
        ResetTime();
            
    }
}
