﻿using System.Collections;
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
    public Text nameField;
    public Text roomField;

    Color color;
    public Image colorSelector;

    private void Update()
    {
        //Change the local color
        if (Input.GetKeyDown(KeyCode.C))
        {
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            colorSelector.color = color;
            PhotonManagerExample.instance.playerColor = color;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Application.Quit();
        }
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomField.text))
        {
            PhotonManagerExample.instance.CreateRoom(roomField.text);
        }
        else
        {
            PhotonManagerExample.instance.CreateRoom();
        }
    }
    public void JoinRandomRoom()
    {
        PhotonManagerExample.instance.JoinRandomRoom();
    }
    public void JoinChatroom()
    {
        if(!string.IsNullOrEmpty(nameField.text))
        {
            Debug.Log("[MenueUI][JoinChatroom] Joining Chatroom ");
            PhotonManagerExample.instance.username = nameField.text;

            if (nameField.text != null)
            {
                PhotonManagerExample.instance.JoinChatroom();
            }
        }
        else
        {
            Debug.Log("[MenueUI][JoinChatroom] No Username ");
        }
    }

    public void JoinGame()
    {
        if (!string.IsNullOrEmpty(nameField.text))
        {
            Debug.Log("[MenueUI][JoinGame] Joining Game ");

            if (nameField.text != null)
            {
                PhotonManagerExample.instance.username = nameField.text;
                PhotonNetwork.NickName = nameField.text;
            }

            PhotonManagerExample.instance.JoinGame();
        }
        else
        {
            Debug.Log("[MenueUI][JoinGame] No Username ");
        }
    }
}
