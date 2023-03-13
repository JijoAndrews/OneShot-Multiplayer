using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpwanManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{


    public static SpwanManager instance;
    public bool IsgameStarted,isVr;
    public GameObject playerPrefab,ragDollPrefab,movCamRef,blackScreen,beginScreen,respwanScreen,escapePanel;
    public int colorId;
    public TMP_Text bulletTxt,infoTxt,timeTxt;
    private GameObject player,ragDoll;
    public PhotonView spwanPhotonview;
    public Transform[] spwanPostions;
    public List<int> charColorVal;
    public List<string> infoStringlist;
    
    public float mins, secs,timeLimit;
    public int escapeInt;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            ResetTime();

            SoundManager.instance.myAudioSource.volume =0f;
            SoundManager.instance.ReverbSounds(false,1f);

            Debug.Log("color value from custom property-" + PhotonNetwork.NickName + "--" + PhotonNetwork.LocalPlayer.CustomProperties["ColorId"]);

            colorId = (int)PhotonNetwork.LocalPlayer.CustomProperties["ColorId"];
            
            //SpwanPlayer();


            //if (!IsgameStarted)
            //{
            //    blackScreen.SetActive(true);
            //    beginScreen.SetActive(true);
            //}
        }

        if (!IsgameStarted)
        {
            //TimeManager.instance.ResetTime();
            StartCoroutine(EnableInfo(true,0.5f));
        }
    }


    public void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    void Update()
    {


        if (mins < 3)
        {
            secs += 1f * Time.deltaTime;

            if (secs >= 1)
            {
                //blackScreen.SetActive(true);
                //beginScreen.SetActive(true);

                mins += 1f;
                secs = 0f;
                SoundManager.instance.startSound(2,0.3f,1f);
                IsgameStarted = mins == 3 ? true : false;

                timeTxt.text = (4 - mins).ToString();
                infoTxt.text = infoStringlist[(int)(mins - 1)];

                if (mins == 3)
                {
                    StartCoroutine(EnableInfo(false, 1f));
                    
                }
            }

        }



        if(spwanPhotonview.IsMine && Input.GetKeyDown(KeyCode.Escape))
        {
            //if (escapeInt == 0)
            //{
            //    escapePanel.SetActive(true);
            //    escapeInt = 1;
            //}
            //else
            //{
            //    escapePanel.SetActive(false);
            //    escapeInt = 0;
            //}


            EscapePanel(escapeInt);
        }
    }




    IEnumerator EnableInfo(bool curState,float time)
    {
        yield return new WaitForSeconds(time);
        blackScreen.SetActive(curState);
        beginScreen.SetActive(curState);

        Debug.Log("completed");
        if (IsgameStarted &&  PhotonNetwork.IsConnected)
        {
            SpwanPlayer();
            movCamRef.GetComponent<MovCam>().enabled = true;
        }
    }

    public void SpwanPlayer()
    {

      
        int randId = Random.Range(0, spwanPostions.Length);
       // int randList = Random.Range(1, charColorVal.Count);
       // int bRandlist = randList - 1;
      
        object[] playerPreData = new object[]
        {
           // bRandlist
            charColorVal[colorId]
            
        };

        // Debug.Log("frst val-" + charColorVal[randList]);

        if (isVr)
        {
            player = PhotonNetwork.Instantiate("playerAndCamforVR_V1", spwanPostions[randId].position, spwanPostions[randId].rotation, 0, playerPreData);
            player.name = "Player-" + PhotonNetwork.NickName;
        }
        else
        {
            player = PhotonNetwork.Instantiate("PlayerV2", spwanPostions[randId].position, spwanPostions[randId].rotation, 0, playerPreData);
            player.name = "Player-" + PhotonNetwork.NickName;
        }

      
    }



    [PunRPC]
    public void ResetColorList(int listId)
    {
        Debug.Log("the index for removing color from--"+charColorVal.Count+"---removing at--" + listId);
        //charColorVal.RemoveAt(listId);//og
        charColorVal.Remove(listId);
    }


    public void HandleScore(int score)
    {
        bulletTxt.text = score.ToString();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
       // throw new System.NotImplementedException();

    }


    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    Debug.Log("Entered player color id-" +newPlayer.NickName+"--"+ newPlayer.CustomProperties["ColorId"]);
    //}

    public void TimeToReswpan(Transform playerTransform)
    {
        blackScreen.SetActive(true);
        respwanScreen.SetActive(true);
        ResetTime();
        HandleScore(1);
        //movCamRef.GetComponent<MovCam>().RemoveAllReference();
        //movCamRef.GetComponent<MovCam>().enabled = false;
       
        Debug.Log("Dead and respwanned");
        if (PhotonNetwork.IsConnected)
        {

            //movCamRef.GetComponent<MovCam>().enabled = true;

            //blackScreen.SetActive(false);
            //respwanScreen.SetActive(false);



            object[] playerPreData = new object[]
             {
         
                 charColorVal[colorId]

             };

            // Debug.Log("frst val-" + charColorVal[randList]);
            Vector3 curPos = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y - 1f, playerTransform.localPosition.z);
            ragDoll = PhotonNetwork.Instantiate("RagDollV2",curPos,playerTransform.rotation,0,playerPreData);
            ragDoll.name = "PlayerRagadoll-" + PhotonNetwork.NickName;




            StartCoroutine(PlayerRespwanned());
        }

    }


    IEnumerator PlayerRespwanned()
    {

        yield return new WaitForSeconds(5f);
        blackScreen.SetActive(false);
        respwanScreen.SetActive(false);
        SpwanPlayer();
    }



    public void EscapePanel(int Val)
    {


        if (Val == 0 || escapeInt==0)
        {
            Debug.Log("is Paused----" + Val);
            escapePanel.SetActive(true);
            escapeInt = 1;
            player.GetComponent<PlayerMovement>().isPaused = true;
        }

        //else 
        //{
        //    escapePanel.SetActive(false);
        //    escapeInt = 0;
        //}


        if(Val==1)
        {
            Debug.Log("is Paused----" + Val);
            escapePanel.SetActive(false);
            escapeInt = 0;
            player.GetComponent<PlayerMovement>().isPaused = false;
        }

        if (spwanPhotonview.IsMine && Val==3)
        {
            Debug.Log("is Paused----" + Val);
           player.GetComponent<PlayerMovement>().LeaveGame();
            StartCoroutine(BacktoHomePage());
        }
    }




    IEnumerator BacktoHomePage()
    {
        blackScreen.SetActive(true);
        escapePanel.SetActive(false);
        respwanScreen.SetActive(true);
        respwanScreen.transform.GetChild(1).GetComponent<TMP_Text>().text = "Leaving the Game..";
        yield return new WaitForSeconds(2f);
        SoundManager.instance.myAudioSource.volume = 0.3f;

        PhotonNetwork.LoadLevel(0);
    }
}
