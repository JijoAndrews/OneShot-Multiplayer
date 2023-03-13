using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class PlayerMovement :MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    float playerHeight=2f;
    [SerializeField] Transform orientation;
    [SerializeField] Animator playerAnimControler;
    [SerializeField] GameObject bulletPos,bulletPrefab,bulletRef,aimImg;
    [SerializeField] float xRot;
    [SerializeField] PhotonView myPhotonView;

    [Header("CHARECTER VAL")]
    public int charColorVal;
    [SerializeField] TextMeshPro playerName;
    [SerializeField] List<Color> textColor;
    [SerializeField] List<Material> colorMet;
    [SerializeField] SkinnedMeshRenderer joints,surface;

    [Header("MOVEMENT")]
    [SerializeField] float curSpeed;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float movementMultiplayer = 10f;
    [SerializeField] float airMovementMultiplayer = 0.4f;
    [SerializeField] float horizontalMovement;
    [SerializeField] float verticalMovement;

    [Header("SPRINTING")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceralation = 10f;

    [Header("JUMP")]
    public float jumpSpeed = 5f;

    [Header("DRAG")]
    [SerializeField] float groundDrag = 5f;
    [SerializeField] float airDrag = 2f;

    [Header("KEYBINDS")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;

    [Header("BULLET")]
    public int bulletinGun;
    [SerializeField] TextMeshProUGUI bulletTxt;
    public BulletScript myBullet;

    [Header("CAMERA REFERENCE")]
    public LineRenderer aimLine;
    public GameObject hitRef, orientationRef, sphereBallRef,aimObj,camObj,burtPart;
    public Transform camPosRef, frntObjRef, backObjRef, centrObjRef,movobjRef,groundCheckRef;

    [Header("GROUND DETECTION")]
    [SerializeField] Transform groundCheckTrans;
    [SerializeField] LayerMask groundMask;
    public bool isDead,isGrounded,isIdle,isWalking,isRunning,isStrafe,jumpRayEnabled,isJumping,isGameStarted,isPaused,isVr;
    float groundDistance = 0.4f;

    public Rigidbody rb;
    Vector3 movDirection;
    Vector3 slopeMoveDirection;
    RaycastHit slopeHit;

    public TimeManager myTimeManager;
    public static PlayerMovement instance;


  private void Start()
  {
        instance = this;
        rb=GetComponent<Rigidbody>();
        rb.freezeRotation=true;
        bulletinGun = 1;
        playerName.text = myPhotonView.Owner.NickName;

        if (!myPhotonView.IsMine)
        {
            gameObject.name = "PlayerClone-" +myPhotonView.Owner.NickName;
            gameObject.tag = "Enemy";     
        }



        if (bulletTxt != null)
        {
            bulletTxt.text = bulletinGun.ToString();

        }


        if (camObj == null && myPhotonView.IsMine)
        {
            camObj = FindObjectOfType<MovCam>().gameObject;
            //aimImg = camObj.GetComponent<MovCam>().aimImg;
            SetupCamera();
        }

    }

    private void Update()
  {
        if (myPhotonView.IsMine && !isPaused)
        {

            isGrounded = Physics.CheckSphere(groundCheckTrans.position, groundDistance, groundMask);
            MyInput();
            ControlDrag();
            ControlSpeed();

            if (Input.GetKeyDown(jumpKey) && isGrounded)
            {
                JumpPlayer();
                jumpRayEnabled = true;
            }


            if (jumpRayEnabled)
            {
                Ray jumpingRay = new Ray(transform.position, Vector3.down * playerHeight / 2f); 
                if (!Physics.Raycast(jumpingRay.origin, jumpingRay.direction, (playerHeight / 2f) + 0.2f))
                {
                    Debug.DrawRay(jumpingRay.origin, jumpingRay.direction, Color.white);
                    isJumping = true;
                }
                else
                {
                    Debug.DrawRay(jumpingRay.origin, jumpingRay.direction, Color.red);
                    isJumping = false;
                }
            }

            Ray aimRay = new Ray(bulletPos.transform.position, (aimObj.transform.position - bulletPos.transform.position).normalized);
            aimLine.SetPosition(0, aimObj.transform.position);
            aimLine.SetPosition(1, bulletPos.transform.position);

            Color c1 = textColor[charColorVal];
            Color c2 = textColor[charColorVal];
            aimLine.startColor = textColor[charColorVal];
            aimLine.endColor = textColor[charColorVal];
            Debug.DrawRay(aimRay.origin, aimRay.direction, Color.red);

            if (Input.GetKeyDown(shootKey) && isIdle && bulletinGun == 1)
            {


                //----------------------local bullet transformation---------------------\\
                //GameObject bullet = Instantiate(bulletPrefab);
                //bullet.name = "BulletDawwwwwww";
                //float r = Mathf.Atan2(bulletPos.transform.position.x - aimObj.transform.position.x, bulletPos.transform.position.y - aimObj.transform.position.y);
                //float d = (r / Mathf.PI) * 180f;
                //bullet.GetComponent<BulletScript>().IntializePosDirection(bulletPos.transform.position, aimRay.direction, d, bulletRef);


                float r = Mathf.Atan2(bulletPos.transform.position.x - aimObj.transform.position.x, bulletPos.transform.position.y - aimObj.transform.position.y);
                float d = (r / Mathf.PI) * 180f;

                object[] playerPreData = new object[]
                {
                    charColorVal,
                    myPhotonView.Owner.NickName
                                   
                };
               

                SoundManager.instance.startSound(0, 0f, 2f);
                SoundManager.instance.startSound(3, 0f, 2f);
                GameObject spwanBullet = PhotonNetwork.Instantiate("SphereV2",bulletPos.transform.position, Quaternion.identity,0,playerPreData);
                spwanBullet.name = "Bullet-"+PhotonNetwork.NickName;
                spwanBullet.GetComponent<BulletScript>().IntializePosDirection(bulletPos.transform.position, aimRay.direction,d,bulletRef);
                myBullet = spwanBullet.GetComponent<BulletScript>();
                myBullet.ownerplayerRef = this;
                bulletinGun = 0;
                if (myPhotonView.IsMine)
                {
                    SpwanManager.instance.HandleScore(bulletinGun);
                }
                myTimeManager.DoSlowmotion();
            }
            slopeMoveDirection = Vector3.ProjectOnPlane(movDirection, slopeHit.normal);
        }
    }
 

    private void FixedUpdate()
    {
        if (myPhotonView.IsMine)
        {
            MovePlayer();
        }

        if (!photonView.IsMine)
        {
            playerName.gameObject.transform.LookAt(Camera.main.transform);

        }
    }

    private bool SlopeHitCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2f + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
   
    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        movDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }
  
    void MovePlayer()
    {
        if(horizontalMovement==0f && verticalMovement==0f)
        {
            curSpeed = 0f;
            isIdle = true;
            isRunning = false;
            isWalking = false;
            aimObj.SetActive(true);
        }
        else if(Input.GetKey(sprintKey) && !isJumping && bulletinGun<1) 
        {
            isIdle = false;
            isWalking = false;
            isRunning = true;
            curSpeed = 1f;

            aimObj.SetActive(false);
        }
        else if(isJumping)
        {
            isRunning = false;
            isIdle = false;
            isWalking = false;
            curSpeed = 1f;
            aimObj.SetActive(true);

        }
        else
        {
            isRunning = false;
            isIdle = false;
            isWalking = true;
            curSpeed = 1f;

            aimObj.SetActive(false);
        }


        if (isGrounded &&!SlopeHitCheck())
        {
            rb.AddForce(movDirection.normalized * moveSpeed * movementMultiplayer, ForceMode.Acceleration);
           //Debug.Log("movCheck--" + moveSpeed);
        }
        else if (isGrounded && SlopeHitCheck())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplayer, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(movDirection.normalized * moveSpeed * movementMultiplayer * airMovementMultiplayer, ForceMode.Acceleration);
        }



        if (horizontalMovement == -1|| horizontalMovement == 1)
        {
            isStrafe = true;
            playerAnimControler.SetFloat("Sidecontrol",horizontalMovement *1f);

        }
        else
        {
            isStrafe = false;
        }

        playerAnimControler.SetBool("IsSideMov", isStrafe);


        // if (verticalMovement == -1)
        // {

        //     playerAnimControler.SetFloat("SpeedControl", -1f);
        
        // }else if (verticalMovement == 1)
        // {
        //     playerAnimControler.SetFloat("SpeedControl", 1f);

        // }

        playerAnimControler.SetFloat("SpeedControl", verticalMovement);

        //isIdle =rb.velocity==Vector3.zero;
        playerAnimControler.SetBool("IsMoving",isWalking);
        playerAnimControler.SetBool("IsRunning", isRunning);
        
    }

    void JumpPlayer()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up*jumpSpeed,ForceMode.Impulse);
        }
    }
   
    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void ControlSpeed()
    {
        if(Input.GetKey(sprintKey) && isGrounded &&bulletinGun<1)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceralation * Time.deltaTime);
            isRunning = true;
            isWalking = false;
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceralation * Time.deltaTime);
            isRunning = false;
        }
    }


    public void OnTriggerEnter(Collider other)
    {    
        if (other.gameObject.transform.parent.gameObject.name == "BulletClone")
        {
           Debug.Log("Enemy hit--" +other.gameObject.name +"--player photon view-" + myPhotonView.IsMine);
            DeadAndRespwan();
        }
    }


    public void DeadAndRespwan()
    {

        if (!myPhotonView.IsMine)
        {
            Debug.Log("particle on--");
            burtPart.SetActive(true);
            burtPart.GetComponent<ParticleSystem>().Play();
        }


        if (myPhotonView.IsMine)
        {
            isDead = true;
            bulletinGun = 1;

           // SoundManager.instance.startSound(6, -2, 2f);    
            SpwanManager.instance.TimeToReswpan(transform);
            PhotonNetwork.Destroy(gameObject);
           //playerAnimControler.gameObject.SetActive(false);
            
        }
    }

    public void SetupCamera()
    {
        camObj = FindObjectOfType<MovCam>().gameObject;
        camObj.GetComponent<MovCam>().myPlayer = GetComponent<PlayerMovement>();
    }

    public void testRespwan() 
    {
        photonView.RPC("testRespwanName", RpcTarget.AllViaServer);
    
    }

   
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantionData = info.photonView.InstantiationData;

        charColorVal = (int)instantionData[0];

        Debug.Log("char color is got from spwan-" + charColorVal);
        Material[] mats = joints.materials;
        mats[0] = colorMet[charColorVal];
        joints.materials = mats;


        Material[] mats1 = surface.materials;
        mats1[0] = colorMet[charColorVal];
        surface.materials = mats1;

        playerName.color = textColor[charColorVal];

        if (!photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }

    }
   
    [PunRPC]
    void testRespwanName()
    {
        if (myPhotonView.IsMine)
        {
            Debug.Log("finnaly got him");
            SoundManager.instance.startSound(6, -2, 2f);


            if (myBullet != null)
            {
                myBullet.SelfDestroy();

            }
            DeadAndRespwan();

        }

        if (!myPhotonView.IsMine)
        {
            Debug.Log("the hit on the sound");
        }
    }

    [PunRPC]
    void NamethePlayer(string name)
    {
        gameObject.name = "PlayerClone-" + name;
    }

    IEnumerator GetToSlowmo()
    {
        yield return new WaitForSeconds(0.2f);
        myTimeManager.DoSlowmotion();
    }

    public void TriggerSlowMoition()
    {
        myTimeManager.DoSlowmotion();
    }

    public void QuickUndoSlowmotion()
    {
        myTimeManager.UndoSlowmotion();
    }

    public void LeaveGame()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.DestroyAll(gameObject);
    }


}
