using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    public Text healthText;
    public Text nameText;

    void Update()
    {
        if(Camera.current)
        {
            transform.LookAt(Camera.current.transform.position);
        }
    }
}
