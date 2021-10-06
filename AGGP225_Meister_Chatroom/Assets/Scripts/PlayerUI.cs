using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    public GameObject targetholder;
    PlayerController target;
    public Text healthText;
    public Text nameText;

    private void Start()
    {
        target = targetholder.GetComponent<PlayerController>();
        //nameText.text = target.username;
    }
    void Update()
    {
        if(Camera.current)
        {
            transform.LookAt(Camera.current.transform.position);
        }
    }

    public void SetTarget(PlayerController _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }

        // Cache references for efficiency
        target = _target;
        if (nameText != null)
        {
            nameText.text = PhotonNetwork.NickName;
        }
    }
}
