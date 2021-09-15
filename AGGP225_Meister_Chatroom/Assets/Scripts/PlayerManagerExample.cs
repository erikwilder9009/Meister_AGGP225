﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerManagerExample : MonoBehaviour
{
    public Color color;

    private void Start()
    {
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void Update()
    {
        //Changes color over the network
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetPhotonView().RPC("ChangeColor", RpcTarget.AllBuffered, color.r, color.g, color.b);
        }

        //Change the local color
        if (Input.GetKeyDown(KeyCode.C))
        {
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        
    }


    [PunRPC]
    void ChangeColor(float r, float g, float b)
    {
        Color c = new Color(r, g, b);
        Camera.main.backgroundColor = c;
    }
}
