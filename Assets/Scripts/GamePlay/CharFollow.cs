using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class CharFollow : MonoBehaviour
{

    [SerializeField] Transform orientiation;
    [SerializeField] Quaternion rot;
    [SerializeField] PhotonView myPlayerPhotonView;
    public bool isVr;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(myPlayerPhotonView.IsMine && !isVr)
        transform.rotation = Quaternion.Lerp(transform.rotation, orientiation.rotation, 15f * Time.deltaTime);//15
        //rot = Quaternion.Lerp(rot, orientiation.localRotation, 10f * Time.deltaTime);
    }
}
