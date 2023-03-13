using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MovCam : MonoBehaviour
{
    public GameObject aimImg;
    [SerializeField] GameObject playerRef,hit,oritentation,sphereBall,AimObj;
    [SerializeField] Transform cameraPostion,frntObj,backObj,centrObj,MovObj;
    [SerializeField] float frntDist,backDist,xRotVal,tempXrot,tempVal,TargetDist,curFdist,curBdist,frntDistCoverd,curfVal,curbVal,zVal,mfVal,mbVal;
    [SerializeField] Slider slideVal;
    [SerializeField] bool isMouseMoving;
    [SerializeField] Quaternion curPos,rotval;
    [SerializeField] Vector3 minDist;
    [SerializeField] PhotonView playerPhotonView;
    public bool isSetuped,isVr;
    public PlayerMovement myPlayer;
    // Start is called before the first frame update



   void Awake()
    {
        Debug.Log("Awake");
    }


    private void OnEnable()
    {
        Debug.Log("Enable");
      

        //if (playerRef == null)
        //{
        //    playerRef = GameObject.FindGameObjectWithTag("Player");
        //}


        //if (playerRef != null)
        //{
        //    hit = playerRef.GetComponent<PlayerMovement>().hitRef;
        //    oritentation = playerRef.GetComponent<PlayerMovement>().orientationRef;
        //    sphereBall = playerRef.GetComponent<PlayerMovement>().sphereBallRef;

        //    cameraPostion = playerRef.GetComponent<PlayerMovement>().camPosRef;
        //    frntObj = playerRef.GetComponent<PlayerMovement>().frntObjRef;
        //    backObj = playerRef.GetComponent<PlayerMovement>().backObjRef;
        //    centrObj = playerRef.GetComponent<PlayerMovement>().centrObjRef;
        //    MovObj = playerRef.GetComponent<PlayerMovement>().movobjRef;
        //    AimObj = playerRef.GetComponent<PlayerMovement>().aimObj;
        //}


        //frntDist = Mathf.Round(Vector3.Distance(frntObj.localPosition, hit.transform.localPosition) * 100f);
        //backDist = Mathf.Round(Vector3.Distance(backObj.localPosition, hit.transform.localPosition) * 100f);
        //zVal = hit.transform.localPosition.z;
    }


    void Start()
    {
      

        //if (playerRef == null)
        //{
        //    playerRef = GameObject.FindGameObjectWithTag("Player");
        //}


        //if (playerRef != null)
        //{
        //    hit = playerRef.GetComponent<PlayerMovement>().hitRef;
        //    oritentation = playerRef.GetComponent<PlayerMovement>().orientationRef;
        //    sphereBall = playerRef.GetComponent<PlayerMovement>().sphereBallRef;

        //    cameraPostion = playerRef.GetComponent<PlayerMovement>().camPosRef;
        //    frntObj = playerRef.GetComponent<PlayerMovement>().frntObjRef;
        //    backObj = playerRef.GetComponent<PlayerMovement>().backObjRef;
        //    centrObj = playerRef.GetComponent<PlayerMovement>().centrObjRef;
        //    MovObj = playerRef.GetComponent<PlayerMovement>().movobjRef;
        //    AimObj = playerRef.GetComponent<PlayerMovement>().aimObj;
        //}


        //frntDist = Mathf.Round(Vector3.Distance(frntObj.localPosition, hit.transform.localPosition) * 100f);
        //backDist = Mathf.Round(Vector3.Distance(backObj.localPosition, hit.transform.localPosition) * 100f);

        //zVal = hit.transform.localPosition.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
       

        if (myPlayer!=null)
        {

            transform.position = cameraPostion.position;


            rotval = Quaternion.Normalize(transform.rotation);

            curFdist = Mathf.Round(Vector3.Distance(frntObj.localPosition, hit.transform.localPosition) * 100f);
            curBdist = Mathf.Round(Vector3.Distance(backObj.localPosition, hit.transform.localPosition) * 100f);

            tempVal = (hit.transform.position - frntObj.transform.position).magnitude;

            if (gameObject.GetComponent<RectTransform>().eulerAngles.x > 90)//recttransform
            {
                float xVal = gameObject.GetComponent<RectTransform>().eulerAngles.x;//recttransform
                xRotVal = Mathf.Round(xVal - 360f);

            }
            else if (gameObject.GetComponent<RectTransform>().eulerAngles.x <= 90)//recttransform
            {
                xRotVal = Mathf.Round(gameObject.GetComponent<RectTransform>().eulerAngles.x);//recttransform

            }


            frntDistCoverd = frntDist - curFdist;

            if (xRotVal > 0f)
            {
                curbVal = 0f;

                hit.transform.localPosition = Vector3.Lerp(hit.transform.localPosition, centrObj.transform.localPosition, 5f * Time.deltaTime);

                if (xRotVal <= 90f)
                {
                    // minDist = new Vector3(backObj.transform.localPosition.x,backObj.transform.localPosition.y,MovObj.transform.localPosition.z-(tempVal / xRotVal));
                    tempXrot = 91f - xRotVal;
                    mfVal = Mathf.Abs(Mathf.Clamp(xRotVal / 90f, 0f, 90f));

                    float fv = Mathf.Abs(Mathf.Clamp(mfVal, 0f, tempVal));
                    float dist = (sphereBall.transform.position - frntObj.transform.position).magnitude;

                    TargetDist = Mathf.Abs(Mathf.Clamp(xRotVal / 10f, 0f, 10f));

                    minDist = new Vector3(frntObj.transform.localPosition.x, frntObj.transform.localPosition.y, centrObj.transform.localPosition.z + (tempVal * mfVal));
                    //  sphereBall.transform.localPosition = minDist;
                    //  sphereBall.transform.localPosition = Vector3.Lerp(sphereBall.transform.localPosition, minDist, 10f * Time.deltaTime);



                    if (xRotVal < curfVal)
                    {

                    }
                    else
                    {
                        curfVal = xRotVal;
                    }

                    // slideVal.value = xRotVal;        
                    //hit.transform.localPosition = Vector3.Lerp(hit.transform.localPosition, minDist,5f * Time.deltaTime);

                }
            }
            else if (xRotVal < 0f)
            {
                curfVal = 0f;
                mfVal = 0f;
                TargetDist = 0f;
                tempXrot = 0f;

                //  hit.transform.localPosition = Vector3.Lerp(hit.transform.localPosition, centrObj.transform.localPosition,5f * Time.deltaTime);
                if (xRotVal >= -90f)
                {
                    //  minDist = new Vector3(backObj.transform.localPosition.x, backObj.transform.localPosition.y, MovObj.transform.localPosition.z + tempVal / xRotVal);

                    mbVal = Mathf.Abs(Mathf.Clamp(xRotVal / 90f, -90f, 90f));
                    // Debug.Log(mbVal);
                    float fv = Mathf.Abs(Mathf.Clamp(mbVal, 0f, tempVal));

                    minDist = new Vector3(backObj.transform.localPosition.x, backObj.transform.localPosition.y, centrObj.transform.localPosition.z - (tempVal * mbVal));
                    // sphereBall.transform.localPosition = minDist;
                    // sphereBall.transform.localPosition = Vector3.Lerp(sphereBall.transform.localPosition, minDist, 10f * Time.deltaTime); 


                    if (xRotVal > curbVal)
                    {

                    }
                    else
                    {
                        curbVal = xRotVal;

                    }


                    //slideVal.value = xRotVal;
                    // hit.transform.localPosition = Vector3.Lerp(hit.transform.localPosition, minDist, 5f * Time.deltaTime);

                }
            }
            else
            {
                //  hit.transform.localPosition = Vector3.Lerp(hit.transform.localPosition, centrObj.transform.localPosition, 10f * Time.deltaTime);
            }

            sphereBall.transform.localPosition = Vector3.Lerp(sphereBall.transform.localPosition, minDist, 20f * Time.deltaTime);//tempMov//10
            //aimImg.transform.position = transform.InverseTransformPoint(AimObj.transform.position);
            // aimImg.transform.localPosition = Camera.main.WorldToViewportPoint(AimObj.transform.position);
        }

    }


    //private void LateUpdate()
    //{
    //    sphereBall.transform.localPosition = Vector3.Lerp(sphereBall.transform.localPosition, minDist, 10f * Time.deltaTime);//tempMov

    //}


    public void RemoveAllReference()
    {

        playerRef = null;
        hit = null;
        oritentation = null;
        sphereBall = null;
        cameraPostion = null;
        frntObj = null;
        backObj = null;
        centrObj = null;
        MovObj = null;
        AimObj = null;
    }




    private void Update()
    {
        if (myPlayer && !isVr)
        {
            RemoveAllReference();
            //if (playerRef == null)
            //{
            //    playerRef = GameObject.FindGameObjectWithTag("Player");
            //}

            //if (playerRef != null)
            //{
                hit = myPlayer.hitRef;
                oritentation = myPlayer.orientationRef;
                sphereBall = myPlayer.sphereBallRef;

                cameraPostion = myPlayer.camPosRef;
                frntObj = myPlayer.frntObjRef;
                backObj = myPlayer.backObjRef;
                centrObj = myPlayer.centrObjRef;
                MovObj = myPlayer.movobjRef;
                AimObj = myPlayer.aimObj;
          //  }


            frntDist = Mathf.Round(Vector3.Distance(frntObj.localPosition, hit.transform.localPosition) * 100f);
            backDist = Mathf.Round(Vector3.Distance(backObj.localPosition, hit.transform.localPosition) * 100f);

         
            zVal = hit.transform.localPosition.z;

        }
    }
}
