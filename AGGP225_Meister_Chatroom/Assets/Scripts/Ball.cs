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
        if (gameObject.GetPhotonView().IsMine && live)
        {
            Debug.Log("thrown :: " + transform.forward * 1000000000000000000f);
            rb.AddForce(transform.forward * 1000000000000000000f);
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
