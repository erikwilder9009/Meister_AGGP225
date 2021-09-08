using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class MenueUI : MonoBehaviour
{
    public static PhotonManagerExample instance { get; private set; }

    [SerializeField]
    public Text inputField;

    public void CreateRoom()
    {
        PhotonManagerExample.instance.CreateRoom();
    }
    public void JoinRandomRoom()
    {
        PhotonManagerExample.instance.JoinRandomRoom();
    }
    public void JoinChatroom()
    {
        if(!string.IsNullOrEmpty(inputField.text))
        {
            Debug.Log("[MenueUI][JoinChatroom] Joining Chatroom ");
            PhotonManagerExample.instance.username = inputField.text;

            if (inputField.text != null)
            {
                PhotonManagerExample.instance.JoinChatroom();
            }
        }
        else
        {
            Debug.Log("[MenueUI][JoinChatroom] No Username ");
        }
    }
}
