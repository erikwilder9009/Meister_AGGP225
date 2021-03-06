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
    public GameObject hat;

    int gameMode;

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
                PlayerController character = PhotonNetwork.Instantiate(playerPrefab.name, Spawns[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position, Quaternion.identity).GetComponent<PlayerController>();
                Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
                if(PhotonManagerExample.instance.teammatch)
                {
                    if (PhotonNetwork.LocalPlayer.ActorNumber == 1 || PhotonNetwork.LocalPlayer.ActorNumber == 2)
                    {
                        character.teamnum = 1;
                    }
                    else
                    {
                        character.teamnum = 2;
                    }
                }
                else
                {
                    character.teamnum = 0;
                }

                Debug.Log(PhotonManagerExample.instance.hatname);
                Debug.Log(hat.name);
                if(PhotonManagerExample.instance.hatname == hat.name)
                {
                    Debug.Log("Yes");
                }
                hat = PhotonNetwork.Instantiate(PhotonManagerExample.instance.hatname, character.hatHolder.transform.position, character.hatHolder.transform.rotation);
                character.hatHolder.transform.localPosition = hat.transform.position;
                hat.transform.parent = character.hatHolder.transform;
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


    public void spawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Spawns[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position, Quaternion.identity).GetComponent<PlayerController>();
    }

    public void leaveGame()
    {
        PhotonNetwork.LoadLevel("Chatroom");
    }
}
