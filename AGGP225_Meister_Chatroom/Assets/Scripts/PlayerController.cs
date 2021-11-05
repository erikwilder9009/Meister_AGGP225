using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

using cakeslice;


public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public GameObject head;
    float headRot;

    public GameObject bullet;
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

    int lives;

    int gameMode; //1=deathmatch, 2=team deathmatch, 3=normal team match

    void Start()
    {
        lives = 3;
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


    GameObject highlightObj;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(head.transform.position, head.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if(hit.transform.gameObject.GetComponent<cakeslice.Outline>())
            {
                highlightObj = hit.transform.gameObject;
                highlightObj.transform.gameObject.GetComponent<cakeslice.Outline>().enabled = true;
                Debug.DrawRay(head.transform.position, head.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                
            }
            else
            {
                if(highlightObj != hit.transform.gameObject && highlightObj !=null)
                {
                    highlightObj.transform.gameObject.GetComponent<cakeslice.Outline>().enabled = false;
                }
                Debug.DrawRay(head.transform.position, head.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
                
            }
        }
        else
        {
            if(highlightObj != null)
            {
                highlightObj.transform.gameObject.GetComponent<cakeslice.Outline>().enabled = false;
            }
            Debug.DrawRay(bulletSpawn.transform.position, bulletSpawn.transform.TransformDirection(Vector3.forward) * 1000, Color.red);
        }



        if (gameObject.GetPhotonView().IsMine)
        {
            if (Input.GetButtonDown("Jump") && grounded)
            {
                rb.AddForce(transform.up * 750);
                grounded = false;
            }


            if (Input.GetButtonDown("Fire1") && holdingBall)
            {
                bullet.transform.parent = null;
                bullet.GetComponent<Ball>().live = true;
                bullet.GetComponent<Ball>().Throw();
                bullet.GetComponent<Rigidbody>().isKinematic = false;
                audioS.PlayOneShot(shot);
                holdingBall = false;
            }


            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                if(Input.GetButton("Fire3"))
                {
                    rb.velocity = (transform.forward * (Input.GetAxis("Vertical") * 20) + new Vector3(0, rb.velocity.y, 0) + transform.right * (Input.GetAxis("Horizontal") * 15));
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



            if (health <= 0 && (gameMode == (1 | 2) || lives <= 0))
            {
                Destroy(gameObject); 
                audioS.PlayOneShot(ouch);
                PhotonNetwork.LoadLevel("Chatroom");
            }
            if(health <= 0 && gameMode == 3)
            {
                lives -= 1;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
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

        if (collision.gameObject.GetComponent<Ball>())
        {
            if (collision.gameObject.GetComponent<Ball>().live)
            {
                gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, 1);
                collision.gameObject.GetComponent<Ball>().live = false;
            }
            else
            {
                holdingBall = true;
                bullet = collision.gameObject;
                bullet.GetComponent<Rigidbody>().isKinematic = true;
                bullet.transform.position = bulletSpawn.transform.position;
                bullet.transform.rotation = bulletSpawn.transform.rotation;
                bullet.transform.parent = bulletSpawn.transform;
            }

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
