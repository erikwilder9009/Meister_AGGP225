using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;
using Photon;
using Photon.Chat;

public class PhotonManagerExample : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    string gameVersion = "1";
    RoomOptions roomOptions = new RoomOptions();
    static string gameplayLevel = "Game Level";

    public bool teammatch;

    public string username;
    public Material playerMat;
    public string hatname;

    public GameObject ConnectedUI;
    public static PhotonManagerExample instance { get; private set; }


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        roomOptions.MaxPlayers = 4;
    }


    private void Start()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        Connect();


    }


    /// <summary>
    /// Connects user to Master 
    /// </summary>
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void JoinRandomRoom()
    {
        Debug.Log("[PhotonManager][JoiningRandomRoom]");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[PhotonManager][ConnectedToMaster]");
        ConnectedUI.SetActive(true);
    }
    public void CreateRoom()
    {
        Debug.Log("[PhotonManager][CreatingRoom] Trying to create room....");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }
    public void CreateRoom(string Name)
    {
        Debug.Log("[PhotonManager][CreatingRoom] Trying to create room : " + Name);
        PhotonNetwork.CreateRoom(Name, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("[PhotonManager][OnCreatedRoom]");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("[PhotonManager][OnJoinedRoom]");
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameplayLevel);
        }
    }

    public void JoinChatroom()
    {
        gameplayLevel = "Chatroom";
        PhotonNetwork.JoinRandomRoom();

    }

    public void JoinGame()
    {
        Debug.Log("[PhotonManager][JoinGame]");
        gameplayLevel = "FPS";
        PhotonNetwork.JoinRandomRoom();
    }

    public void LoadMenue()
    {
        gameplayLevel = "MainMenu";
        PhotonNetwork.LeaveRoom();
    }
    public void LoadLobby()
    {
        gameplayLevel = "Chatroom";
        PhotonNetwork.JoinOrCreateRoom("Lobby", null, null);
    }
    public void LoadTeamLobby()
    {
        gameplayLevel = "TeamChatroom";
        PhotonNetwork.JoinOrCreateRoom("TeamLobby", null, null);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
        Destroy(gameObject);
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[PhotonManager][Disconected] " + cause);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("[PhotonManager][CreateRoomFailed] " + message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("[PhotonManager][JoinRoomFailed] " + message);
        CreateRoom();
    }


    public void SetNickname(string nickname)
    {
        gameObject.GetPhotonView().Owner.NickName = nickname;
    }



    [PunRPC]
    void ChangeColor(float r, float g, float b)
    {
        playerMat.color = new Color(r, g, b);
    }
}