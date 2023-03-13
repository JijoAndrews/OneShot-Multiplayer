using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerLook : MonoBehaviour
{
    [Header("REFERENCE")]
    [SerializeField] WallRun wallRun;
    [SerializeField] GameObject targetBone,playerObj;
    [SerializeField] Transform playerRef,topLook,bottomLook,PostionRef;
    [SerializeField] PhotonView playerPhotonView;
    public bool isVr;

    float mouseX;
    float mouseY;

    float multiplayer = 0.01f;

    float xRotation;
    float yRotation;

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

   [SerializeField] Transform myCam,orientation;
    [SerializeField] Vector3 Pos;
    [SerializeField] Quaternion playerRot;




    private void Start()
    {

        if (myCam == null && !isVr)
        {
            myCam = FindObjectOfType<MovCam>().gameObject.transform;
        }

    }

    private void Update()
    {
        if (playerPhotonView.IsMine && !isVr)
        {
            MyMouseInputs();
            Pos = PostionRef.position;
            myCam.transform.localRotation = Quaternion.Euler(xRotation,yRotation,wallRun.tilt);
            orientation.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            playerRot = Quaternion.Euler(0f, 0f, wallRun.playerRot);
            //playerRef.rotation = Quaternion.Euler(0f, 0f, wallRun.playerRot);
        }
    }

    void MyMouseInputs()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplayer;
        xRotation -= mouseY * sensY * multiplayer;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


      //playerObj.transform.localRotation = Quaternion.Euler(xRotation + 90f, 0f, 0f);

    }
}
