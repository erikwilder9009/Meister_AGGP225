using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;


public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public GameObject head;
    float headRot;

    GameObject bullet;
    public GameObject bulletSpawn;
    bool grounded;
    public Camera PlayerCamera;

    public GameObject hatHolder;

    public string username;
    public int health;

    public PlayerUI ui;
    
    AudioSource audioS;
    public AudioClip shot;
    public AudioClip step;
    bool walking;
    float walkdelay = .5f;
    float walkset;
    public AudioClip ouch;


    bool holdingBall;


    void Start()
    {
        walkset = Time.time;
        audioS = gameObject.GetComponent<AudioSource>();
        health = 3;
        gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, 0);
        gameObject.GetPhotonView().RPC("UpdateUI", RpcTarget.AllBuffered);

        if (gameObject.GetPhotonView().IsMine)
        {
            rb = gameObject.GetComponent<Rigidbody>();
            grounded = true;
            PlayerCamera.enabled = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            if (Input.GetButtonDown("Jump") && grounded)
            {
                rb.AddForce(transform.up * 200);
                grounded = false;
            }
            if (Input.GetButtonDown("Fire1") && holdingBall)
            {
                Debug.Log("throw");
                bullet.transform.parent = null;
                bullet.GetComponent<Ball>().enabled = true;
                bullet.GetComponent<Rigidbody>().isKinematic = false;
                audioS.PlayOneShot(shot);
                holdingBall = false;
            }
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                if(Input.GetButton("Fire3"))
                {
                    rb.velocity = (transform.forward * (Input.GetAxis("Vertical") * 10) + new Vector3(0, rb.velocity.y, 0) + transform.right * (Input.GetAxis("Horizontal") * 10)) * 3;
                }
                else
                {
                    rb.velocity = transform.forward * (Input.GetAxis("Vertical") * 10) + new Vector3(0, rb.velocity.y, 0) + transform.right * (Input.GetAxis("Horizontal") * 10);
                }
                walking = true;
            }
            else
            {
                walking = false;
            }
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 5, 0));



            headRot -= Input.GetAxis("Mouse Y");
            if(headRot > -100 && headRot < 40)
            {
                head.transform.localEulerAngles = new Vector3(headRot, 0, 0);
            }
            if(headRot <-100)
            {
                headRot = -99.9f;
            }
            if (headRot > 60)
            {
                headRot = 39.9f;
            }



            if (walking && Time.time > walkset + walkdelay)
            {
                walkset = Time.time;
                audioS.PlayOneShot(step);
            }



            if (health <= 0)
            {
                Destroy(gameObject);
                audioS.PlayOneShot(ouch);
                PhotonNetwork.LoadLevel("Chatroom");
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ball>())
        {
            gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, 1);
            PhotonNetwork.Destroy(other.gameObject);
        }
        if (other.gameObject.GetComponent<ballPickup>() && !holdingBall)
        {
            holdingBall = true;
            bullet = PhotonNetwork.Instantiate("BasicBall", bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            bullet.transform.parent = bulletSpawn.transform;
            PhotonNetwork.Destroy(other.gameObject);
        }
        if (other.gameObject.GetComponent<seekerballPickup>() && !holdingBall)
        {
            holdingBall = true;
            bullet = PhotonNetwork.Instantiate("SeekerBall", bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            bullet.transform.parent = bulletSpawn.transform;
            PhotonNetwork.Destroy(other.gameObject);
        }
        if (other.gameObject.GetComponent<basicPickup>())
        {
            if(health < 3)
            {
                health += 1; 
                gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, 0);
            }
            PhotonNetwork.Destroy(other.gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }


    [PunRPC]
    void UpdateUI()
    {
        ui.nameText.text = gameObject.GetPhotonView().Owner.NickName;
    }


    [PunRPC]
    void TakeDamage(int Damage)
    {
        health -= Damage;
        ui.healthText.text = "HP : " + health.ToString();
    }
}
