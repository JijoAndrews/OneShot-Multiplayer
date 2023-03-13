using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;


public class RagDollScript : MonoBehaviourPunCallbacks,IPunInstantiateMagicCallback
{

    public PhotonView ragdollPV;
    public Animator myAnim;
    public float pumpSpeed;
    public GameObject burstPrefab;
    [SerializeField] int charColorVal;
    [SerializeField] List<Material> colorMet;
    [SerializeField] SkinnedMeshRenderer joints, surface;

    public List<Rigidbody> ragdollRigidBody;

   
    void Start()
    {
       // Invoke("RemoveRagdoll",5f); //uncoment for multiplayer
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))//check function to see if ragdoll is working 
        {
            myAnim.enabled = false;
            foreach (Rigidbody rb in ragdollRigidBody)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            Rigidbody randomRb =  ragdollRigidBody[Random.Range(9, ragdollRigidBody.Count-1)];
            randomRb.AddForce(Vector3.up*pumpSpeed, ForceMode.Impulse);
            randomRb.AddRelativeTorque(Vector3.forward * 20f, ForceMode.Impulse);
           
        }
    }

     public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

        object[] instantionData = info.photonView.InstantiationData;

        charColorVal = (int)instantionData[0];

        Debug.Log("RagDoll Char color is got from spwan-" + charColorVal);
        Material[] mats = joints.materials;
        mats[0] = colorMet[charColorVal];
        joints.materials = mats;


        Material[] mats1 = surface.materials;
        mats1[0] = colorMet[charColorVal];
        surface.materials = mats1;


        foreach (Rigidbody rb in ragdollRigidBody)
        {
            rb.isKinematic = false;
        }

        RagDollInitializie();

    }


    public void RemoveRagdoll()
    {
        if (ragdollPV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


    public void RagDollInitializie()
    {
        myAnim.enabled = false;
        foreach (Rigidbody rb in ragdollRigidBody)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        Rigidbody randomRb = ragdollRigidBody[Random.Range(9, ragdollRigidBody.Count - 1)];
        randomRb.AddForce(Vector3.down * pumpSpeed, ForceMode.Impulse);
        randomRb.AddRelativeTorque(transform.forward * 20f, ForceMode.Impulse);
    }



    public void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag=="Gun" || collision.gameObject.tag == "Bullet")
        //{
        //    Debug.Log("hello--" + collision.gameObject.name);
        //    myAnim.enabled = false;
        //    foreach (Rigidbody rb in ragdollRigidBody)
        //    {
        //        rb.isKinematic = false;
        //        rb.useGravity = true;
        //    }
        //    Rigidbody randomRb = ragdollRigidBody[Random.Range(9, ragdollRigidBody.Count - 1)];
        //    randomRb.AddForce(Vector3.up * pumpSpeed, ForceMode.Impulse);
        //    randomRb.AddRelativeTorque(Vector3.forward * 20f, ForceMode.Impulse);

        //    Invoke("RemoveRagdoll", 5f);

        //}
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gun" || other.gameObject.tag == "Bullet")
        {
            Debug.Log("hello--" + other.gameObject.name);
            myAnim.enabled = false;
            foreach (Rigidbody rb in ragdollRigidBody)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            GameObject burstpart = Instantiate(burstPrefab, ragdollRigidBody[9].transform.position,Quaternion.identity,transform);
            Rigidbody randomRb = ragdollRigidBody[Random.Range(9, ragdollRigidBody.Count - 1)];
            randomRb.AddForce(Vector3.up * pumpSpeed, ForceMode.Impulse);
            randomRb.AddRelativeTorque(Vector3.forward * 20f, ForceMode.Impulse);

            Invoke("RemoveRagdoll", 5f);

        }
    }
}
