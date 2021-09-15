using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class PhotonManagerExample : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    RoomOptions roomOptions = new RoomOptions();
    static string gameplayLevel = "Game Level";

    public string username;

    public static PhotonManagerExample instance { get; private set; }


    void Awake()
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

        PhotonNetwork.AutomaticallySyncScene = true;
        roomOptions.MaxPlayers = 4;
    }


    private void Start()
    {
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
    }
    public void CreateRoom()
    {
        Debug.Log("[PhotonManager][CreatingRoom] Trying to create room....");
        PhotonNetwork.CreateRoom(null, roomOptions);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("[PhotonManager][OnCreatedRoom]");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("[PhotonManager][OnJoinedRoom]");

        PhotonNetwork.LoadLevel(gameplayLevel);
    }

    public void JoinChatroom()
    {
        gameplayLevel = "Chatroom";
        PhotonNetwork.JoinRandomRoom();
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


    [PunRPC]
    void UsernameRPC(string _username, string _chat)
    {
        PhotonChatManager.instance.chatBox.text += "\n" + _username + " :  " + _chat;
    }

}