using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introManager : MonoBehaviour
{

    public Animator bot1Animator,bot2Animator;
    public AnimatorStateInfo animInfo;
    public GameObject idlePos,gunonHand,gunonHip,bullet,bulletPart,aimPos,headRef;
    public float dist;
    public bool startShooting,idleStanceState,isShootingState,isGunonHand;

    void Start()
    {
        animInfo = bot2Animator.GetCurrentAnimatorStateInfo(0);
    }

    void Update()
    {
        IncommingBotFromSide();
    }


    public void IncommingBotFromSide()
    {
        idleStanceState = bot2Animator.GetCurrentAnimatorStateInfo(0).IsName("IdleStance");
        isShootingState = bot2Animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting");


        if (!idleStanceState && !isGunonHand)
        {
            StartCoroutine(TakeGun(idleStanceState));
        }

        if (bullet.activeSelf == true)
        {
            bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, headRef.transform.position, 5f * Time.deltaTime);

        }
    }


    IEnumerator TakeGun(bool stat)
    {
        isGunonHand = true;
        yield return new WaitForSeconds(0.2f);
        gunonHip.SetActive(stat);
        gunonHand.SetActive(!stat);
        Debug.Log("current setting ---" + "---gun on hip--" + gunonHip.activeSelf + "---gun on hand---" + gunonHand.activeSelf);
        TimeSlowForever();
    }

    public void TimeSlowForever()
    {
        StartCoroutine(WaitAndStopForever());
    }

    IEnumerator WaitAndStopForever()
    {
        yield return new WaitForSeconds(1.4f);
        bulletPart.transform.position = bullet.transform.position;
        bulletPart.SetActive(true);
        SoundManager.instance.startSound(7, 0, 2f);
        SoundManager.instance.ReverbSounds(true, 0.7f);
        bullet.SetActive(true);
        Time.timeScale = 0.005f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void ResetTime()
    {
        bot2Animator.speed = 0f;
        Time.timeScale =1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }
}
