using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Realtime;

using TMPro;

public class RoomBtn : MonoBehaviour
{

    public TMP_Text buttonTxt;
    private RoomInfo info;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetbtnDetails(RoomInfo inputInfo)
    {
        info = inputInfo;
        buttonTxt.text = info.Name;
    }


    public void OpenRoom()
    {
        MyLauncher.instance.JoinRoom(info);
    }
}
