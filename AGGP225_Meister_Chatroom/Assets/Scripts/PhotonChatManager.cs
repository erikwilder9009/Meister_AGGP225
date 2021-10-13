using Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class PhotonChatManager : MonoBehaviour
{
    public static PhotonChatManager instance;
    public InputField chatInput;
    public Text chatBox;
    string username;
    public Color color;
    bool chatSelected;

    // Start is called before the first frame update
    void Start()
    {
        chatSelected = false;
        instance = this;
        username = PhotonManagerExample.instance.username;
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (chatSelected)
            {
                SendChat();
                chatInput.DeactivateInputField();
                Cursor.visible = false;
                chatSelected = false;
            }
            else
            {
                chatInput.ActivateInputField();
                Cursor.visible = true;
                chatSelected = true;
            }
        }
    }

    public void SendChat()
    {
        if (PhotonManagerExample.instance && PhotonManagerExample.instance.gameObject.GetPhotonView())
        {
            gameObject.GetPhotonView().RPC("ChatRPC", RpcTarget.AllBuffered, username, chatInput.text);
        }
        else
        {
            Debug.Log("[PhotonChatManager] PhotonChatManagerExample has no PhotonView");
        }
        chatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string _username, string _chat)
    {
        PhotonChatManager.instance.chatBox.text += "\n" + _username + " :  " + _chat;
    }


    public void LoadMenue()
    {
        PhotonManagerExample.instance.LoadMenue();
    }
}
