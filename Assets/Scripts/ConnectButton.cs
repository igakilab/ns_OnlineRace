using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviourPunCallbacks
{

    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        //ボタンを無効に
        button.interactable = false;
    }

    // ロビーへの接続成功
    public override void OnJoinedLobby()
    {
        //ボタンを有効に
        button.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
