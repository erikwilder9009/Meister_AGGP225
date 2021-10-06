using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;
using Photon.Realtime;

public class GameplayManagerExample : MonoBehaviour
{
    public static GameplayManagerExample instance { get; private set; }
    public GameObject playerPrefab;
    public Material playerMat;
    private void Start()
    {
        instance = this;
        Cursor.visible = false;

        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("MainMenu");

            return;
        }

        if(instance == this)
        {
            if (playerPrefab)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
                gameObject.GetPhotonView().RPC("ChangeColor", RpcTarget.AllBuffered);
            }
            else
            {
                Debug.Log("[GameplayManager] the variable 'playerPrefab' is not set");
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            PhotonManagerExample.instance.LoadMenue();
        }
    }

    [PunRPC]
    void ChangeColor()
    {
        playerMat.color = PhotonManagerExample.instance.playerColor;
    }

}
