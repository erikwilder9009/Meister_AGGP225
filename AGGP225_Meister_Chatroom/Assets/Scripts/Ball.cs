using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            rb.velocity += transform.forward * 2.5f;
        }
    }
}
