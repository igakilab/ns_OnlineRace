using Photon.Pun;

public class ConnectServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // マスターサーバーに接続
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ロビーに接続
        PhotonNetwork.JoinLobby();
    }
}
