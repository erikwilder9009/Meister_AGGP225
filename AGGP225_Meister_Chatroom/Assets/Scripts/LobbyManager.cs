using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public List<Text> playernames;

    bool joining;
    bool done;
    float joinLimit = 10;
    public Text joinDisplay;

    private void Awake()
    {
        joining = false;
        done = false;
        joinLimit = 10;
        joinDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            playernames[p.ActorNumber - 1].text = p.NickName;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 4 && joining == false)
        {
            Debug.Log("[LobbyManager][JoinGame] all players in lobby");
            joining = true;
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


        if (joining && !done)
        {
            joinDisplay.gameObject.SetActive(true);
            joinLimit -= 1 * Time.deltaTime;
            joinDisplay.text = joinLimit.ToString("F0");

            if (joinLimit <= 0)
            {
                JoinGame();
                done = true;
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
