using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


using Photon.Pun;
using Photon.Realtime;

public class UsernameExample : MonoBehaviour
{
    public static UsernameExample instance;
    public Text username;

    public void Awake()
    {
        instance = this;

        username.text = PhotonManagerExample.instance.username;
        PhotonManagerExample.instance.gameObject.GetPhotonView().RPC("UsernameRPC", RpcTarget.AllBuffered,PhotonManagerExample.instance.username.ToString(),"Hello World");
    }
}
