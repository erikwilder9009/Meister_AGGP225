using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Ball : MonoBehaviour
{
    public bool live;
    Rigidbody rb;
    // Start is called before the first frame update
    public virtual void Start()
    {
        live = true;
        rb = gameObject.GetComponent<Rigidbody>();

        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.Find("MiddleLine(wall)").GetComponent<Collider>(), true);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (gameObject.GetPhotonView().IsMine && live)
        {
            rb.velocity += transform.forward * .5f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        live = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        live = false;
    }
}
