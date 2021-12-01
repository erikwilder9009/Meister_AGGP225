using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

using cakeslice;

public class Ball : MonoBehaviour
{
    public bool live;
    Rigidbody rb;
    // Start is called before the first frame update
    public virtual void Start()
    {
        GetComponent<Outline>().enabled = false;

        rb = gameObject.GetComponent<Rigidbody>();

        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.Find("MiddleLine(wall)").GetComponent<Collider>(), true);
    }

    public void Throw()
    {
        if (gameObject.GetPhotonView().IsMine && !live)
        {
            live = true;
            transform.parent = null;
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 10000);
            GetComponent<Collider>().enabled = true;
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
