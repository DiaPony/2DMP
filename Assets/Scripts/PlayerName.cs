using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{
    public InputField nameTF;
    public Button setNameBtn;

    public Button createRoomBTN;
    public Button joinRoomBTN;

    public void OnTFChange()
    {
        if (nameTF.text.Length > 2)
        {
            setNameBtn.interactable = true;
        }
        else
        {
            setNameBtn.interactable = false;
        }
    }

    public void OnClick_SetName()
    {
        PhotonNetwork.NickName = nameTF.text;
        createRoomBTN.interactable = true;
        joinRoomBTN.interactable = true;
    }
}
