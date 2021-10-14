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
    public GameObject bullet;
    public GameObject bulletSpawn;
    bool grounded;
    public Camera PlayerCamera;

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

    void Start()
    {
        walkset = Time.time;
        audioS = gameObject.GetComponent<AudioSource>();
        health = 20;
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
                rb.AddForce(transform.up * 300);
                grounded = false;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject bullet = PhotonNetwork.Instantiate("CannonBall", bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                Destroy(bullet, 2);
                audioS.PlayOneShot(shot);
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
            head.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * 5, 0, 0));

            if(walking && Time.time > walkset + walkdelay)
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
        if (other.gameObject.tag == "Bullet")
        {
            gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, 1);
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
