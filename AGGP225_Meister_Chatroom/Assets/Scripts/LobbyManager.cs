using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public List<Text> playernames;

    private void Awake()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            playernames[p.ActorNumber - 1].text = p.NickName;
        }
        gameObject.GetPhotonView().RPC("UsernameRPC", RpcTarget.AllBuffered, PhotonNetwork.NickName);
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            Debug.Log("[LobbyManager][JoinGame] all players in lobby");
            JoinGame();
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount > 4)
        {
            Debug.Log("[LobbyManager] Too many players in room...");
            PhotonManagerExample.instance.LoadMenue();
            foreach(Player p in PhotonNetwork.PlayerList)
            {
                Debug.Log(p);
            }
        }
    }

    public void JoinGame()
    {
        Debug.Log("[LobbyManager][JoinGame] joining");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("FPS");
        }
    }

    [PunRPC]
    void UsernameRPC(string _username)
    {
        playernames[PhotonNetwork.CurrentRoom.PlayerCount - 1].text = _username;
    }
}
