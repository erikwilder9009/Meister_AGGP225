using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class MenueUI : MonoBehaviour
{
    public void CreateRoom()
    {
        PhotonManagerExample.instance.CreateRoom();
    }
    public void JoinRandomRoom()
    {
        PhotonManagerExample.instance.JoinRandomRoom();
    }
}
