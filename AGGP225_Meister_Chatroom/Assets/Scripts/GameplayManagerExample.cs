using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon;
using Photon.Pun;
using Photon.Realtime;

public class GameplayManagerExample : MonoBehaviour
{
    public float timerLimit;
    public Text timerDisplay;
    bool timerDone;

    public List<GameObject> Spawns;

    public static GameplayManagerExample instance { get; private set; }
    public GameObject playerPrefab;
    public Material playerMat;
    private void Start()
    {
        timerDone = false;
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
                PhotonNetwork.Instantiate(playerPrefab.name, Spawns[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position, Quaternion.identity).GetComponent<PlayerController>();
                gameObject.GetPhotonView().RPC("ChangeColor", RpcTarget.AllBuffered, PhotonManagerExample.instance.playerColor.r, PhotonManagerExample.instance.playerColor.g, PhotonManagerExample.instance.playerColor.b);
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

        timerLimit -= 1 * Time.deltaTime;
        timerDisplay.text = timerLimit.ToString("F0");

        if(timerLimit <= 0 && !timerDone)
        {
            PhotonNetwork.LoadLevel("Chatroom");
            timerDone = true;
        }
    }

    [PunRPC]
    void ChangeColor(float r, float g, float b)
    {
        playerMat.color = new Color(r, g, b);
    }

}
