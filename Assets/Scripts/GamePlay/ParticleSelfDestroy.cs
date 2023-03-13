using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class ParticleSelfDestroy : MonoBehaviourPunCallbacks,IPunInstantiateMagicCallback
{

    public PhotonView myBurstPhotonview;
    public int colorId;
    public Color[] colorList;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

        object[] instantionData = info.photonView.InstantiationData;
        colorId = (int)instantionData[0];
        var main = GetComponent<ParticleSystem>().main;
        main.startColor = colorList[colorId];

        if (myBurstPhotonview.IsMine)
        {

            Invoke(nameof(selfDestroy),5f);
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(selfDestroy), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void selfDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }

}
