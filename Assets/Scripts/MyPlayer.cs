using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{
    public PhotonView photonview;
    public float moveSpeed = 10;

    public Text playerName;

    private Vector3 smoothmove;

    private GameObject sceneCamera;
    public GameObject playerCamera;

    public Transform shootingPoint;
    public Transform arm;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    public float shootCooldown;
    private float currentTimer;

    public int HP;
    public int currentHP;

    public Image HPbar;

    public int Damage;

    
    private void Start()
    {
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 15;

        if (photonView.IsMine)
        {
            sceneCamera = GameObject.Find("Main Camera");
            playerName.text = PhotonNetwork.NickName;
            

            sceneCamera.SetActive(true);
            playerCamera.SetActive(true);

            currentHP = HP;

        }
        else
        {
            playerName.text = photonView.Owner.NickName;
        }


    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            ProcessInputs();
            photonView.RPC("FillHPBar", RpcTarget.All);
        }
        else
        {
            SmoothMovement();
        }
    }
    void SmoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothmove, Time.deltaTime * 10);
    }
    [PunRPC]
    void FillHPBar()
    {
        HPbar.fillAmount = (float)currentHP / (float)HP;
    }

    [PunRPC]
    public void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().playerWhoShot = gameObject;
        rb.AddForce(shootingPoint.up * bulletForce, ForceMode2D.Impulse);
    }
   

    void ProcessInputs()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position += move * moveSpeed * Time.deltaTime;

        if (currentTimer <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                photonView.RPC("Shoot", RpcTarget.All);
                currentTimer = shootCooldown;
            }
        }
        else
        {
            currentTimer -= Time.deltaTime;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);            
        }
        else if (stream.IsReading)
        {
            smoothmove = (Vector3)stream.ReceiveNext();
        }
       
    }
}
