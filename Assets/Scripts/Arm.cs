using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arm : MonoBehaviourPun, IPunObservable
{

    public PhotonView photonview;

    private Quaternion rotationForOtherPlayers;

    public GameObject firepoint;
    public GameObject otherFirepoint;

    private Vector2 mousePosition;
    private Vector2 transformPos;
    public Camera Cam;

    public float angle;



    private void Start()
    {
        Cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (photonview.IsMine)
        {
            getMousePos();
            getTransformPos();

           
            angle = AngleBetweenTwoPoints(transformPos, mousePosition);

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90f));
        }
         
    }

    private void FixedUpdate()
    {
        if (photonview.IsMine)
        {
            lookToMouse();
        }
        else
        {
            transform.rotation = rotationForOtherPlayers;
        }
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void lookToMouse()
    {
        Vector3 lookDir = mousePosition - (Vector2)transform.position;
        

        transform.rotation = Quaternion.Euler(lookDir.x, lookDir.y, 0);
    }

    private void getMousePos()
    {
        mousePosition = Cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void getTransformPos()
    {
        transformPos = Cam.ScreenToViewportPoint(transform.position);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
            stream.SendNext(firepoint);
        }
        else if (stream.IsReading)
        {
            rotationForOtherPlayers = (Quaternion)stream.ReceiveNext();
            otherFirepoint = (GameObject)stream.ReceiveNext();
        }
    }

   
}
