using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    // Start is called before the first frame update
    public string ownerName;
    public PlayerMovement ownerplayerRef;
    public GameObject bulletPartBurst;
    [SerializeField] float bulletSpeed,xRot;
    [SerializeField] Vector3 intialDir;
    [SerializeField] Rigidbody myRigidbody;
    [SerializeField] Collider hitColl;
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] bool getCaught;
    Vector3 dirChange;
    public Material[] colorMet;
    public Color[] colorList;

    [SerializeField] PhotonView bulletPhotonView;
    void Start()
    {

            


        if (!bulletPhotonView.IsMine)
        {
            gameObject.name = "BulletClone";
        }
        Invoke("EnableTrail",0.02f);
    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!bulletPhotonView.IsMine)
        {
            myRigidbody.isKinematic = true;
        }
        else
        {
            gameObject.name = "Bullet-" + ownerName;
        }

        object[] instantionData = info.photonView.InstantiationData;

        int colorId = (int)instantionData[0];
            ownerName = (string)instantionData[1];

        bulletTrail.startColor = colorList[colorId];
        
        Debug.Log("Bullet color is got from spwan-" + colorId);
        
        Material[] mats = bulletTrail.materials;
        mats[0] = colorMet[colorId];
        bulletTrail.materials = mats;



    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [PunRPC]
    void NametheBullet(string name)
    {
        Debug.Log("rpc triggred");
        gameObject.name = "Bullet Clone-"+ name;
    }


    public void IntializePosDirection(Vector3 pos,Vector3 dir,float rot,GameObject bulletRef)
    {
        //Debug.Log(pos + "---" + dir);
        intialDir = dir;
        dirChange = dir;
        transform.position = pos;
        xRot = rot;
        transform.rotation = bulletRef.transform.rotation;

        // myRigidbody.AddForce(dir* bulletSpeed * Time.deltaTime);

        if (myRigidbody!=null)
        {
            myRigidbody.velocity = (dir) * bulletSpeed;

        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Obstacles")
        {
           // Debug.Log("got Hit--" + collision.gameObject.tag);
            bulletTrail.enabled = false;
            ChangeDir(intialDir, collision);
        }

        if (!bulletPhotonView.IsMine)
        {
            //Debug.Log("got Hit--" + collision.gameObject.tag);
        }

        bulletTrail.enabled = false;
        ChangeDir(intialDir, collision);

    }


    public void ChangeDir(Vector3 nextDir,Collision col)
    {
        //float randX = Random.Range(-40f, 60f);

        //float randY = Random.Range(25f, 50f);

        //float randZ = Random.Range(5f, -95f);



        float randX = Random.Range(-1f, 1f);

        float randY = Random.Range(-1f, 1f);

        float randZ = Random.Range(-1f, 1f);

         Vector3 randDir =  Vector3.Reflect(new Vector3(randX, randY, randZ).normalized,col.GetContact(0).normal).normalized;

        dirChange = randDir;
        // Vector3 randDir = new Vector3(randX, randY, randZ).normalized;
        // myRigidbody.AddForce(randDir * bulletSpeed * Time.deltaTime);






        if (!myRigidbody.isKinematic)
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.velocity = (randDir) * bulletSpeed;
        }

        bulletTrail.enabled = true;

    }



    public void EnableTrail()
    {
        bulletTrail.enabled = true;
        //hitColl.gameObject.SetActive(true);
    }


    public void LateUpdate()
    {
        if (myRigidbody.isKinematic)
        {

            Vector3 latePos = transform.position + dirChange * bulletSpeed * Time.deltaTime;


            transform.position = Vector3.Lerp(transform.position, latePos, 1f * Time.deltaTime);
            
           // transform.position += dirChange * bulletSpeed * Time.deltaTime;//the OG
        }
    }


    public void OnTriggerEnter(Collider other)
    {

      //  Debug.Log("caught my bullet back--" + other.gameObject.name + "--and its parent--" + other.gameObject.transform.parent.name);


        if (other.gameObject.transform.parent.name == "Player-" + ownerName && bulletPhotonView.IsMine)
        {
            Debug.Log("caught my bullet back--" + other.gameObject.name + "--and its parent--" + other.gameObject.transform.parent.name);

            SoundManager.instance.startSound(4,0f,2f);
            other.gameObject.transform.parent.GetComponent<PlayerMovement>().bulletinGun = 1;
            SpwanManager.instance.HandleScore(1);
            PhotonNetwork.Destroy(gameObject);
        }


        //if (!bulletPhotonView.IsMine)
        //{
        //    Debug.Log("caught my bullet back--" + other.gameObject.name + "--and its parent--" + other.gameObject.transform.parent.name + "--with tag--" + other.gameObject.transform.parent.gameObject.tag);

        //}



        if (other.gameObject.transform.parent.name == "Player-" + ownerName && !bulletPhotonView.IsMine)
        {
          //  Debug.Log("caught my bullet back--" + other.gameObject.name + "--and its parent--" + other.gameObject.transform.parent.name + "--with tag--" + other.gameObject.transform.parent.gameObject.tag);

            //other.gameObject.transform.parent.GetComponent<PlayerMovement>().bulletinGun = 1;
            //SpwanManager.instance.HandleScore(1);
            //PhotonNetwork.Destroy(gameObject);
        }


        //if (other.gameObject.transform.parent.gameObject.tag == "Player"&& !bulletPhotonView.IsMine)
        //{
        //    Debug.Log("got hit by bullet--" + other.gameObject.name + "--and its parent--" + other.gameObject.transform.parent.name);


        //    other.gameObject.transform.parent.GetComponent<PlayerMovement>().DeadAndRespwan();
        //    PhotonNetwork.Destroy(gameObject);

        //}






        if (other.gameObject.transform.parent.gameObject.tag == "Enemy" && bulletPhotonView.IsMine)
        {
            Debug.Log("bullet getting triggred by--" + other.gameObject.name + "--and its parent--" + other.gameObject.transform.parent.name);
            SoundManager.instance.RandomHit(0, 0, 2f);//

            // other.gameObject.transform.parent.GetComponent<PlayerMovement>().DeadAndRespwan();
            // PhotonNetwork.Destroy(gameObject);
            other.gameObject.transform.parent.GetComponent<PlayerMovement>().testRespwan();

              object[] playerPreData = new object[]
              {
                other.gameObject.transform.parent.GetComponent<PlayerMovement>().charColorVal

              };


            ownerplayerRef.TriggerSlowMoition();

            GameObject burtPrt= PhotonNetwork.Instantiate("BurstPrefab", transform.position, transform.rotation,0,playerPreData);
            burtPrt.GetComponent<ParticleSystem>().Play();

            StartCoroutine(ResetTime());
            // PhotonNetwork.Destroy(gameObject);
        }

    }




    


    public void SelfDestroy()
    {
        if (bulletPhotonView.IsMine)
        {
            Debug.Log("SelfDestroyer");
            PhotonNetwork.Destroy(gameObject);
        }
    }


    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(0.05f);
        ownerplayerRef.QuickUndoSlowmotion();

    }
}
