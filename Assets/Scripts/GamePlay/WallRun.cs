using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] Transform playerRef;
    [Header("MOVEMENT")]
    [SerializeField] Transform orientation;

    [Header("WALL DETECTION")]
    [SerializeField] float wallDist=.8f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [Header("WALL RUNNING")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallJumpForce;

    [Header("CAMERA EFFECTS")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float wallRunFovTime;

    [SerializeField] private float cameraTilt;
    [SerializeField] private float cameraTiltTime;

    [SerializeField] private float playerTilt;
    [SerializeField] private float playerTiltTime;

    public float tilt { get; private set;}
    public float playerRot{ get; private set; }



    bool isWallOnLeft =false, isWallOnRight=false;
    private Rigidbody rb;
    private RaycastHit leftWallHit,rightWallHit;

    public bool enableWallRunPower=true,isVr;
    bool CanWeWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }
    void CheckWall()
    {
        Debug.DrawRay(transform.position, -orientation.right * wallDist,Color.red);
        Debug.DrawRay(transform.position,  orientation.right * wallDist, Color.red);

        isWallOnLeft = Physics.Raycast(transform.position, -orientation.right,out leftWallHit, wallDist);
        isWallOnRight = Physics.Raycast(transform.position, orientation.right,out rightWallHit, wallDist);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
       
        if (cam == null)
        {
            if (!isVr)
            {
                cam = Camera.main;
            }
            else
            {
                //vr Cam
            }
        }
    }

    private void Update()
    {
        CheckWall();
        if (CanWeWallRun() && enableWallRunPower)
        {
            if (isWallOnLeft)
            {
                StartWallRun();
                //Debug.Log("Wall is on left");
            }
            else if (isWallOnRight)
            {
                StartWallRun();
               // Debug.Log("Wall is on Right");
            }
            else
            {
                StopWallRun();
            }

        }
        else
        {
            StopWallRun();
        }
    }


    void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if (isWallOnLeft)
        {
            tilt = Mathf.Lerp(tilt, -cameraTilt, cameraTiltTime * Time.deltaTime);
            playerRot = Mathf.Lerp(playerRot, -playerTilt, playerTiltTime * Time.deltaTime);

        }
        else if (isWallOnRight)
        {
            tilt = Mathf.Lerp(tilt, cameraTilt, cameraTiltTime * Time.deltaTime);
            playerRot = Mathf.Lerp(playerRot,playerTilt, playerTiltTime * Time.deltaTime);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallOnLeft)
            {
                Vector3 wallJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallJumpDirection * wallJumpForce * 100f, ForceMode.Force);
            }else if (isWallOnRight)
            {
                Vector3 wallJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallJumpDirection * wallJumpForce * 100f, ForceMode.Force);
            }

            playerRot = 0f;
        }
    }

    void StopWallRun()
    {
        rb.useGravity = true;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt,0f, cameraTiltTime * Time.deltaTime);
        playerRot = Mathf.Lerp(playerRot,0f,playerTiltTime * Time.deltaTime);

    }
}
