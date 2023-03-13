using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;


using TMPro;


public class MyLauncher : MonoBehaviourPunCallbacks,IMatchmakingCallbacks,IPunInstantiateMagicCallback
{

    public static MyLauncher instance;

    private void Awake()
    {
       instance = this;
    }

    public PhotonView myPView;
    public int colorId;
    public List<int> colorVal;
    public GameObject loadingScreen,mainTitles,title;
    public TMP_Text loadingText;
    public GameObject menuButtons;


    public GameObject createRoomScene;
    public TMP_InputField roomNameInput;

    public GameObject roomScreen,startBtn;
    public TMP_Text roomNameText,playerNameLabel,colorIdTest;
    private List<TMP_Text> allPlayerNames = new List<TMP_Text>();

    public GameObject errorScreen;
    public TMP_Text errorText;

    public GameObject roomBrowserScreen;
    public RoomBtn theRoomBtn;
    public List<RoomBtn> allRoomButtons = new List<RoomBtn>();

    public GameObject nameInputScreen,curTimeManager;
    public TMP_InputField nameInput;
    private bool hasSetNickName;

    public string levelToPlay;
    int roomCount;
    public List<string> roomListstr;

    Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && nameInputScreen.activeSelf==false)
        {
            GameIntialize();
        }
    }


    void Start()
    {
        //CloseMenus();
        //loadingScreen.SetActive(true);
        //loadingText.text = "Connecting to the network";
        //PhotonNetwork.ConnectUsingSettings();

        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}
        //else
        //{
        //    Destroy(this);
        //}


    }


    public override void OnConnectedToMaster()
    {

        PhotonNetwork.JoinLobby();
        loadingText.text = "joinning Lobby....";

        PhotonNetwork.AutomaticallySyncScene=true;

        if (PhotonNetwork.IsMasterClient)
        {
            startBtn.SetActive(true);
            Debug.Log("Master Client---from onconnectedtoMaster" + PhotonNetwork.IsMasterClient);
        }
        else
        {
            startBtn.SetActive(false);
        }

    }

    public override void OnJoinedLobby()
    {

        CloseMenus();
        menuButtons.SetActive(true);
        PhotonNetwork.NickName = Random.Range(0, 1000).ToString();

        if (!hasSetNickName)
        {
            CloseMenus();
            nameInputScreen.SetActive(true);

            if (PlayerPrefs.HasKey("PlayerName"))
            {
                nameInput.text = PlayerPrefs.GetString("PlayerName");
            }
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
        }



        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties["ColorId"] != null)
            {
                Debug.Log(player.NickName + "--color Id---" + (int)player.CustomProperties["ColorId"]);
               // ResetColor((int)player.CustomProperties["ColorId"]);
            }
        }






        //if (colorVal.Count != 0)
        //{
        //    int randList = Random.Range(0, colorVal.Count - 1);
        //    colorId = colorVal[randList];
        //    Hashtable myplayerVals = new Hashtable();
        //    myplayerVals.Add("ColorId",colorId);
        //    PhotonNetwork.SetPlayerCustomProperties(myplayerVals);
        //    myPView.RPC("ResetColor",RpcTarget.AllBufferedViaServer,colorId);
        //    ResetColor(colorId);
        //}


        //Debug.Log("number of players-" + PhotonNetwork.PlayerList.Length);
        // Debug.Log("number of players-" + PhotonNetwork.CountOfPlayersOnMaster);


        //if (colorVal.Count > 0)
        //{
        //    myPView.RPC("ResetColor", RpcTarget.AllBufferedViaServer, colorId);
        //}


        //foreach(Player player in PhotonNetwork.PlayerList)



        // myPView.RPC("AddColor", RpcTarget.AllBufferedViaServer, PhotonNetwork.CountOfPlayersOnMaster - 1);

    }



    public override void OnConnected()
    {
      

    }

    private void LocalPlayerAdd()
    {
        string name = PhotonNetwork.NickName;
        TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        newPlayerLabel.text =name;
        Debug.Log("Local player name when entered--" + name);
        newPlayerLabel.gameObject.SetActive(true);
        allPlayerNames.Add(newPlayerLabel);
    }

    private void ListAllPlayers()
    {
        foreach(TMP_Text player in allPlayerNames)
        {
            Destroy(player.gameObject);
        }
        allPlayerNames.Clear();
        Player[] players = PhotonNetwork.PlayerList;
        for(int i = 0; i < players.Length; i++)
        {
            TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
            newPlayerLabel.text = players[i].NickName;
            newPlayerLabel.gameObject.SetActive(true);
            allPlayerNames.Add(newPlayerLabel);
           // print(players[i].NickName + "==" + players[i].ActorNumber + "--" + PhotonNetwork.CountOfPlayers);
        }
    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer) //this is only accesible for remote players.
    {

        TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        newPlayerLabel.text = newPlayer.NickName;
        newPlayerLabel.gameObject.SetActive(true);
        allPlayerNames.Add(newPlayerLabel);

       
        Debug.Log("Entered player color id-" + newPlayer.NickName + "--" + newPlayer.CustomProperties["ColorId"]);
        Debug.Log(" remote player name when entered--" + newPlayer.NickName + "--color val--" + newPlayer.CustomProperties["ColorId"]);
       // roomList.Add(PhotonNetwork.GetCustomRoomList(TypedLobby.Default,))
      //  ResetColor((int)newPlayer.CustomProperties["ColorId"]);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ListAllPlayers();

        object[] playerPreData = new object[]
        {
           //bRandlist
           colorId
        };

       

        //myPView.RPC("AddColor",RpcTarget.All,colorId);

    }

    public override void OnJoinedRoom()
    {
        CloseMenus();
        roomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        ListAllPlayers();
        Debug.Log("the number of rooms--" + PhotonNetwork.CountOfRooms + "--players in that room-" + PhotonNetwork.CurrentRoom.Players.Count);

        if (PhotonNetwork.IsMasterClient)
        {
            startBtn.SetActive(true);
            Debug.Log("Master Client-" + PhotonNetwork.IsMasterClient);
           
            //if (colorVal.Count != 0)
            //{
            //    int randList = Random.Range(0, colorVal.Count - 1);
            //    colorId = colorVal[randList];
            //    object[] playerPreData = new object[]
            //    {
            //          //bRandlist
            //          colorVal[randList]
            //    };

            //    //myPView.RPC("ResetColor",RpcTarget.All,randList);

            //    Hashtable myplayerVals = new Hashtable();
            //    myplayerVals.Add("ColorId", colorId);
            //    PhotonNetwork.SetPlayerCustomProperties(myplayerVals);
            //    //ResetColor(randList);
            //    myPView.RPC("ResetColor",RpcTarget.OthersBuffered,colorId);
            //    ResetColor(colorId);

            //}

        }
        else
        {

            startBtn.SetActive(false);
        }


        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties["ColorId"] != null)
            {
                ResetColor((int)player.CustomProperties["ColorId"]);
            }
        }



        if (colorVal.Count != 0)
        {
            int randList = Random.Range(0, colorVal.Count - 1);
            colorId = colorVal[randList];
            colorIdTest.text = colorId.ToString();
            object[] playerPreData = new object[]
            {

                  colorVal[randList]
            };


            Hashtable myplayerVals = new Hashtable();
            myplayerVals.Add("ColorId", colorId);
            PhotonNetwork.SetPlayerCustomProperties(myplayerVals);
            myPView.RPC("ResetColor", RpcTarget.AllBufferedViaServer, colorId);
        }

        Debug.Log("players in  room-" + PhotonNetwork.PlayerList.Length);

    }
    public override void OnLeftRoom()
    {
        CloseMenus();
        menuButtons.SetActive(true);
        Debug.Log("left room");

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed to create room-" + message;
        CloseMenus();
        errorScreen.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        UpdateChachedRoomList(roomList);

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startBtn.SetActive(true);
        }
        else
        {
            startBtn.SetActive(false);
        }
    }
    public void CreateOrRandomRoom()
    {
        RoomOptions randRoomOption = new RoomOptions();
        randRoomOption.MaxPlayers = 3;         
        loadingText.text = "Joinning Random Room";
        loadingScreen.SetActive(true);
        // PhotonNetwork.JoinOrCreateRoom("quickMatchRoom", randRoomOption,TypedLobby.Default); 

        roomCount = PhotonNetwork.CountOfRooms;


        if (PhotonNetwork.CountOfRooms >0)
        {
            OpenRoomBrowser();
        }
        else
        {
            PhotonNetwork.CreateRoom("quickMatchRoom-" + roomCount, randRoomOption, TypedLobby.Default);

        }




        if (PhotonNetwork.IsMasterClient)
        {
            startBtn.SetActive(true);
        }
        else
        {
            startBtn.SetActive(false);
        }

    }


    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Testing");
    }
    public void GameIntialize()
    {

        mainTitles.SetActive(false);
        CloseMenus();
        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to the network";
        PhotonNetwork.ConnectUsingSettings();

    }


    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 3;
            PhotonNetwork.CreateRoom(roomNameInput.text, options);
            CloseMenus();
            loadingText.text = "Creating Room......";
            loadingScreen.SetActive(true);
        }
    }
    public void OpenRoomCreate()
    {
        CloseMenus();
        createRoomScene.SetActive(true);
    }


    public void OpenRoomBrowser()
    {
        CloseMenus();
        cachedRoomList.Clear();
        roomBrowserScreen.SetActive(true);
    }
    public void CloseRoomBrowser()
    {
        CloseMenus();
        menuButtons.SetActive(true);
    }

    public void LeaveRoom()
    {
      //  myPView.RPC("ResetColor", RpcTarget.AllBufferedViaServer, colorId);
        myPView.RPC("AddColor",RpcTarget.AllBufferedViaServer, colorId);

        PhotonNetwork.LeaveRoom();
        CloseMenus();
        loadingText.text = "Leaving Room";
        loadingScreen.SetActive(true);

        //AddColor(colorId);//testing
    }

    void CloseMenus()
    {
        loadingScreen.SetActive(false);
        menuButtons.SetActive(false);
        createRoomScene.SetActive(false);
        roomScreen.SetActive(false);
        errorScreen.SetActive(false);
        roomBrowserScreen.SetActive(false);
        nameInputScreen.SetActive(false);
        mainTitles.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
           // DebuggNetwork();
        }
    }
    public void XbuttonBack()
    {
        CloseMenus();
        menuButtons.SetActive(true);

    }


    public void DebuggNetwork()
    {
        Debug.Log("connection--" +PhotonNetwork.MasterClient); 
    }


    public void UpdateChachedRoomList(List<RoomInfo> roomList)
    {
        for(int i = 0; i < roomList.Count; i++)
        {

            RoomInfo curRoomInfo = roomList[i];


            if (curRoomInfo.RemovedFromList)
            {
                cachedRoomList.Remove(curRoomInfo.Name);
            }
            else
            {
                cachedRoomList[curRoomInfo.Name] = curRoomInfo;
            }


        }

        RoomListButtonUpdate(cachedRoomList);
    }


    void RoomListButtonUpdate(Dictionary<string, RoomInfo> cachedRoomList)
    {
       foreach (RoomBtn rb in allRoomButtons)
        {
            Destroy(rb.gameObject);
        }

        allRoomButtons.Clear();
       

        theRoomBtn.gameObject.SetActive(false);
        foreach (KeyValuePair<string, RoomInfo> roomInfo in cachedRoomList)
        {
            RoomBtn newButton = Instantiate(theRoomBtn, theRoomBtn.transform.parent);
            newButton.SetbtnDetails(roomInfo.Value);
            newButton.gameObject.SetActive(true);
            allRoomButtons.Add(newButton);
        }

    }


    public void JoinRoom(RoomInfo info)
    {

        Debug.Log("players in room-" + info.PlayerCount);

        if (info.PlayerCount < 3)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties["ColorId"] != null)
                {
                    ResetColor((int)player.CustomProperties["ColorId"]);
                }
            }



            if (colorVal.Count != 0)
            {
                int randList = Random.Range(0, colorVal.Count - 1);
                colorId = colorVal[randList];
                colorIdTest.text = colorId.ToString();
                object[] playerPreData = new object[]
                {
                  colorVal[randList]
                };


                Hashtable myplayerVals = new Hashtable();
                myplayerVals.Add("ColorId", colorId);
                PhotonNetwork.SetPlayerCustomProperties(myplayerVals);
                myPView.RPC("ResetColor", RpcTarget.AllBufferedViaServer, colorId);
                // ResetColor(colorId);

            }


            PhotonNetwork.JoinRoom(info.Name);
            CloseMenus();
            loadingText.text = "joinningRoom";
            loadingScreen.SetActive(true);
        }
        else
        {
            StartCoroutine(RoomFullinfo());
        }
    }

    public void SetNickName()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {
            PhotonNetwork.NickName = nameInput.text;
            Debug.Log("nickname-" + PhotonNetwork.NickName);

            PlayerPrefs.SetString("PlayerName", nameInput.text);
            CloseMenus();
            menuButtons.SetActive(true);
            hasSetNickName = true;

            LocalPlayerAdd();
        }
    }

    public void StartGame()
    { 
         PhotonNetwork.LoadLevel(levelToPlay);     
    }



    [PunRPC]
    public void ResetColor(int iD)
    {
        Debug.Log("removed color_"+ iD);
        colorVal.Remove(iD);
    }

    [PunRPC]
    public void AddColor(int iD)
    {
        Debug.Log("Added color_" + iD);
        colorVal.Add(iD);
        colorVal.Sort();
        
    }



    [PunRPC]
    public void Check(int iD)
    {
        Debug.Log("Check Id_" + iD);
    }


   public IEnumerator RoomFullinfo()
    {
        loadingText.text = "Room is full...create a new room.";
        loadingScreen.SetActive(true);
        
        yield return new WaitForSeconds(3f);
        CloseMenus();
        OpenRoomBrowser();
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
      // ((IPunInstantiateMagicCallback)instance).OnPhotonInstantiate(info);
    }


    public void QuitGame()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
}
 