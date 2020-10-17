using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviourPunCallbacks
{

    public Button button;
    public Text buttonText;
    public InputField inputName;

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

    // ルームリストが更新された時に呼ばれるコールバック
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            if (info.PlayerCount == 0)
            {
                buttonText.text = "接続";
                button.interactable = true;
            }
            else
            {
                buttonText.text = info.PlayerCount + "/" + info.MaxPlayers;
                //満員ならボタンを無効化する
                button.interactable = (info.PlayerCount < info.MaxPlayers);
            }
        }
    }

    public void OnClick()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
        button.interactable = false;
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        RankingData.ResetRanking();
        PhotonNetwork.LocalPlayer.NickName = inputName.text;
        SceneManager.LoadScene("GameScene");
    }

    // ルームに参加できなかった時に呼ばれるコールバック
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        buttonText.text = "現在接続できません";
    }

    // 新しくルームを作成したときに呼ばれるコールバック
    public override void OnCreatedRoom()
    {
        GameRoomProperty.ResetHashtable();
        GamePlayerProperty.ResetState();
    }

}
