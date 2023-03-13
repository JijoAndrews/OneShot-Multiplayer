using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScriptvr : MonoBehaviourPunCallbacks
{
    public float bulletSpeed;
    public Vector3 intialDir,curMovingDir;
    public Rigidbody myRigidbody;
    public TrailRenderer bulletTrail;
    public bool timeToMove;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        bulletTrail = GetComponent<TrailRenderer>();
        bulletTrail.enabled = true;

    }

    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        if (timeToMove)
        {
            myRigidbody.AddForce(curMovingDir * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
        }
    }




    public void ChangeDir(Vector3 nextDir, Collision col)
    {
       

        float randX = Random.Range(-1f, 1f);

        float randY = Random.Range(-1f, 1f);

        float randZ = Random.Range(-1f, 1f);

        Vector3 randDir = Vector3.Reflect(new Vector3(randX, randY, randZ).normalized, col.GetContact(0).normal).normalized;

        curMovingDir = randDir;


        if (!myRigidbody.isKinematic)
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.velocity = (randDir) * bulletSpeed;
        }

        bulletTrail.enabled = true;

    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Obstacles")
        {
            // Debug.Log("got Hit--" + collision.gameObject.tag);
            bulletTrail.enabled = false;
            ChangeDir(intialDir, collision);
        }

      

        bulletTrail.enabled = false;
        ChangeDir(intialDir, collision);

    }
}
