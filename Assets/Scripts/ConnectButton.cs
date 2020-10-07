using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviourPunCallbacks
{

    public Button button;
    public Text buttonText;

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

    public void OnClick()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("GameScene");
    }
}
