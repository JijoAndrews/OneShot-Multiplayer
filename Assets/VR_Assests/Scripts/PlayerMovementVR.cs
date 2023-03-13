using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovementVR: MonoBehaviourPunCallbacks
{

    public LayerMask myLayer, wallLayer;

    [Header("CAMERA SETUP")]
    [SerializeField] bool standardCamera;
    public Camera mainCam;


    [Header("SCORE UI")]
    public int bulletVal;

    [Header("MOVEMENT")]
    [SerializeField] float horizontalVal;
    [SerializeField] float verticalVal;
    [SerializeField] float horizontalSideVal;
    [SerializeField] float verticalSideVal;

    [SerializeField] float camSensitivity,finatXrot, yRotation, xRotation, mouseX, mouseY, movSpeed,camRotVal;
    public Vector3 mousePosition, camOffset,refPos,bulletDirection,wallJumpDir;
    public Animator playerAnimator;
    public GameObject camTrackRef,orginCube,VrCamObject,bulletPrefab,bulletBurst,bulletOrginPos,leftHandController,rightHandController,vrlefthand,vrrightHand,lefthandTarget,rightHandTarget,gun,gunTarget,gunInHand;
    public TwoBoneIKConstraint overallRig,leftHand,rightHand;
    public XRRayInteractor rightHandRay;

    public bool isJumped, isRunning,isIntro;
    public bool isVr;
    public bool isPlayerOnGround,isPlayerHitGround,playerHitFrntWall,playerHitBackWall;


    [Header("KeyBoardList")]
    public List<KeyCode> allKeys;

    Vector3 movDirection;
    Rigidbody myRigidbody;
    Collider myCollider;



    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        verticalSideVal = 1f;
        horizontalSideVal = 1f;
        mainCam = Camera.main;

        if (isVr)
        {
            mainCam.gameObject.SetActive(false);
            VrCamObject.SetActive(true);
            
        }
        else
        {
            VrCamObject.SetActive(false);
            mainCam.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        isIntro = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shrugg"); 
        orginCube.transform.position = transform.position + new Vector3(0f, -1f, 0f);
        refPos = camTrackRef.transform.position;

        if (!isVr)
        {
            mainCam.transform.position = refPos;
        }
        else
        {
            VrCamObject.transform.position = refPos;
            leftHandController.transform.localPosition += lefthandTarget.transform.localPosition;
            rightHandController.transform.localPosition += rightHandTarget.transform.localPosition;
        }


        playerInput();
      
        isPlayerOnGround = ChecktheGround();

        AimRayforShoot();
        ControlPlayerDrag();
       
        movDirection = transform.forward * (verticalVal) + transform.right * (horizontalVal);
        PlayerMovement();
        WallRayCheck();
        WallRun();
    }

    private void FixedUpdate()
    {
       // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, yRotation, 0f), 10f * Time.deltaTime);
    }

    public void LateUpdate()
    {
        CamSetup();
    }


    public void playerInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        horizontalVal = Input.GetAxisRaw("Horizontal");
        verticalVal = Input.GetAxisRaw("Vertical");
        
        yRotation += mouseX * camSensitivity * 0.01f;
        xRotation -= mouseY * camSensitivity * 0.01f;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (isPlayerHitGround && !isPlayerOnGround)
            {
                wallJump();
            }
            else
            {
                PlayerJump();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
           
           if (rightHandRay.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
           {
                if (raycastHit.collider.gameObject.tag == "Gun" && !gun)
                {
                    Debug.Log("rey hit---" + raycastHit.collider.gameObject.tag);
                    gun = raycastHit.collider.gameObject;
                    gun.transform.SetParent(gunTarget.transform);
                    gun.transform.position = Vector3.Lerp(gun.transform.position, gunTarget.transform.position,1f);
                    gun.SetActive(false);
                    gunInHand.SetActive(true);
                    rightHandTarget.transform.localRotation = Quaternion.Lerp(rightHandTarget.transform.localRotation, Quaternion.Euler(new Vector3(0f, -90f, -90f)), 1f);

                    vrlefthand.transform.parent.GetComponent<Rig>().weight = 0f;
                    
                    //leftHand.weight = 0;
                    //leftHand.gameObject.SetActive(false);
                    //leftHandController.SetActive(false);
                  

                }else if (gun)
                {
                    gunInHand.SetActive(false);
                    gun.transform.SetParent(gameObject.transform.parent);
                    gun.transform.position = bulletOrginPos.transform.position;
                    gun.SetActive(true);
                    gun.GetComponent<Rigidbody>().AddForce(bulletDirection * 10f,ForceMode.Impulse);
                    gun.GetComponent<Rigidbody>().AddRelativeTorque(gun.transform.forward * 100f,ForceMode.Impulse);

                    TimeManager.instance.DoVRSlowmotion();
                    vrlefthand.transform.parent.GetComponent<Rig>().weight = 1f;
                    gun = null;

                    //leftHand.weight = 0;
                    //leftHand.gameObject.SetActive(false);
                    // leftHandController.SetActive(false);


                }
            }





        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && gunInHand.activeSelf==true && bulletVal==1)
        {
            Shoot();         
        }
    }

    public void PlayerMovement()
    {

        //og movement

        if (horizontalVal != 0 || verticalVal != 0)
        {
            isRunning = isPlayerOnGround?true:false;
            myRigidbody.AddForce(movDirection.normalized * movSpeed, ForceMode.Acceleration);
            float velX = Mathf.Clamp(myRigidbody.velocity.x, -70f, 70f);
            float velY = Mathf.Clamp(myRigidbody.velocity.y, -70f, 70f);
            float velZ = Mathf.Clamp(myRigidbody.velocity.z, -70f, 70f);

            myRigidbody.velocity = new Vector3(velX, velY, velZ);

            if (isVr)
            {
                leftHand.weight = 0;
                rightHand.weight = 0;
            }
        }

        if (horizontalVal == 0 && verticalVal == 0)
        {
            isRunning = false;
            myRigidbody.velocity =Vector3.Lerp(myRigidbody.velocity, Vector3.zero,5f * Time.deltaTime);
            myRigidbody.angularVelocity = Vector3.Lerp(myRigidbody.velocity, Vector3.zero, 5f* Time.deltaTime);
            if (isVr)
            {
                leftHand.weight = Mathf.Lerp(leftHand.weight,1,10f*Time.deltaTime);
                rightHand.weight = Mathf.Lerp(rightHand.weight,1,10f * Time.deltaTime);
            }

        }
    
            playerAnimator.SetBool("isRunning", isRunning);

    }


    public void PlayerJump()
    {    
        myRigidbody.AddForce(transform.up * 20f, ForceMode.Impulse);
    }

    public void wallJump()
    {
        myRigidbody.AddForce(wallJumpDir * 40f, ForceMode.Impulse);
    }

    public bool ChecktheGround()
    {  
       Ray groundRay = new Ray(transform.position, Vector3.down);
       RaycastHit hit;

        if (Physics.Raycast(groundRay, out hit, 1.5f))
        {
            if (hit.collider.tag == "Ground")
            {
                Debug.DrawRay(groundRay.origin, groundRay.direction * 1.5f, Color.blue);
                isJumped = false;
                return true;
            }
        }else
        {
            Debug.DrawRay(groundRay.origin, groundRay.direction * 1.5f, Color.red);
            isJumped = true;
            return false;
        }

        return false;
    }

    public void ControlPlayerDrag()
    {
        if (isPlayerOnGround)
        {
            myRigidbody.drag = 6f;
            movSpeed = 30f;
        } else if (!isPlayerOnGround && isPlayerHitGround) 
        {
            movSpeed = 30f;
            myRigidbody.drag = 6f;
        }
        else
        {
            movSpeed = 5f;
            myRigidbody.drag = 1f;
        }
    }

    public void WallRayCheck()
    {
        Ray wallRightRay = new Ray(transform.position,transform.right);
        Ray wallLeftRay = new Ray(transform.position, -transform.right);

        Ray wallFrntRay = new Ray(transform.position, transform.forward);
        Ray wallBackRay = new Ray(transform.position, -transform.forward);


        Debug.DrawRay(wallRightRay.origin, wallRightRay.direction * 1.5f,Color.green);
        Debug.DrawRay(wallLeftRay.origin, wallLeftRay.direction * 1.5f, Color.green);
        Debug.DrawRay(wallFrntRay.origin, wallFrntRay.direction * 1.5f, Color.green);
        Debug.DrawRay(wallBackRay.origin, wallBackRay.direction * 1.5f, Color.green);

        RaycastHit wallHit;
       
        if (Physics.Raycast(wallFrntRay, out wallHit, 1.5f, ~wallLayer))
        {
            Debug.DrawRay(wallFrntRay.origin, wallFrntRay.direction * 1.5f, Color.black);
            playerHitFrntWall = true;
            playerHitBackWall = false;


        }
        else if (Physics.Raycast(wallBackRay, out wallHit, 1.5f, ~wallLayer))
        {
            Debug.DrawRay(wallBackRay.origin, wallBackRay.direction * 1.5f, Color.black);
            playerHitBackWall = true;
            playerHitFrntWall = false;

        }
        else
        {          
            playerHitBackWall = false;
            playerHitFrntWall = false;
        }


        if (Physics.Raycast(wallRightRay, out wallHit, 1.5f, ~wallLayer))
        {
            Debug.DrawRay(wallRightRay.origin, wallRightRay.direction * 1.5f, Color.black);
            camRotVal = 30f;
            wallJumpDir = wallLeftRay.direction.normalized;
            isPlayerHitGround = true;
        }
        else if (Physics.Raycast(wallLeftRay, out wallHit, 1.5f, ~wallLayer))
        {
            Debug.DrawRay(wallLeftRay.origin, wallLeftRay.direction * 1.5f, Color.black);
            camRotVal = -30f;
            wallJumpDir = wallRightRay.direction.normalized;
            isPlayerHitGround = true;
        }
        else{
            camRotVal = 0f;
            isPlayerHitGround = false;
        }

    }


    public void AimRayforShoot()
    {
        if (isVr)
        {
            if (gunInHand.activeSelf == true)
            {
                Ray aimRay = new Ray(gunInHand.transform.position, gunInHand.transform.forward);
                Debug.DrawRay(aimRay.origin, aimRay.direction * 1.5f, Color.cyan);
                bulletDirection = aimRay.direction.normalized;
            }
        }
        else
        {
            Ray aimRay = new Ray(mainCam.transform.position, mainCam.transform.forward);
            Debug.DrawRay(aimRay.origin, aimRay.direction * 1.5f, Color.cyan);
            bulletDirection = aimRay.direction.normalized;
        }

    }

    public void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab,bulletOrginPos.transform.position,Quaternion.identity);
        bulletObj.name = "Bullet";
        bulletVal = 0;

        GameObject bulletPart = Instantiate(bulletBurst, bulletOrginPos.transform.position, Quaternion.identity);
        TimeManager.instance.DoVRSlowmotion();

        if (isVr)
        {
            // bulletDirection = rightHandRay.rayOriginTransform.forward;
           // bulletDirection = rightHandRay.rayOriginTransform.forward.normalized;
            
        }

        bulletObj.GetComponent<BulletScriptvr>().curMovingDir = bulletDirection;
        bulletObj.GetComponent<BulletScriptvr>().timeToMove = true;
    }

    public void WallRun()
    {
        if(!playerHitBackWall && !playerHitFrntWall && !isPlayerOnGround && isPlayerHitGround)
        {
            if (isVr)
            {
                rightHand.weight = 1;
                rightHandTarget.transform.localRotation = Quaternion.Lerp(rightHandTarget.transform.localRotation, Quaternion.Euler(new Vector3(45f, -90f, -90f)), 1f);
            }

                StartCoroutine(SetAndResetGaravity());
        }
        else
        {
            if (isVr)
            {
               // rightHand.weight = 1;
                rightHandTarget.transform.localRotation = Quaternion.Lerp(rightHandTarget.transform.localRotation, Quaternion.Euler(new Vector3(0f, -90f, -90f)), 1f);
            }
        }
    }

    IEnumerator SetAndResetGaravity()
    {
        myRigidbody.useGravity = false;
        yield return new WaitForSeconds(2f);
        myRigidbody.useGravity = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Debug.Log("bullet hit--" + collision.gameObject.name);
            Destroy(collision.gameObject);
            bulletVal = 1;
        }
    }


void CamSetup()
{
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, yRotation, 0f), 10f * Time.deltaTime);

        if (!isVr)
        {
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, Quaternion.Euler(xRotation, yRotation, camRotVal), 10f * Time.deltaTime);

        }
        else
        {
            VrCamObject.transform.rotation = Quaternion.Lerp(VrCamObject.transform.rotation, Quaternion.Euler(xRotation, yRotation, camRotVal), 10f * Time.deltaTime);
        }
}

}
