using Photon.Pun;
using UnityEngine;

public class ConnectServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // マスターサーバーに接続
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("ConnectMaster");
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ロビーに接続
        PhotonNetwork.JoinLobby();
        Debug.Log("JoinLobby");
    }

}
