using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UpdateFirepoint : MonoBehaviourPun, IPunObservable
{



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform);
        }
        else if (stream.IsReading)
        {
            
        }
    }
}
