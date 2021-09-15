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
    public Text Username;
    public Text chatBox;
    string name;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        name = PhotonManagerExample.instance.username;
        Username.text = PhotonManagerExample.instance.username;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SendChat()
    {
        if (PhotonManagerExample.instance && PhotonManagerExample.instance.gameObject.GetPhotonView())
        {
            PhotonManagerExample.instance.gameObject.GetPhotonView().RPC("UsernameRPC", RpcTarget.AllBuffered, name, chatInput.text);
        }
        else
        {
            Debug.Log("[PhotonChatManager] PhotonManagerExample has no PhotonView");
        }
        chatInput.text = "";
    }
}
