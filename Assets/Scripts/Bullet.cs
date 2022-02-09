using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun, IPunObservable
{

    public int ID;
    public GameObject playerWhoShot;
    public int Damage;

    private void Start()
    {
        ID = playerWhoShot.GetComponent<PhotonView>().ViewID;
        Damage = playerWhoShot.GetComponent<MyPlayer>().Damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.GetComponent<PhotonView>().RPC("destroy", RpcTarget.AllBufferedViaServer);
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<MyPlayer>().currentHP -= Damage;
        }
    }

    [PunRPC]

    private void destroy()
    {
        Destroy(this.gameObject);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
             transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}
