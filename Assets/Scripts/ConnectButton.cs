using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
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
        Debug.Log("ConnectButton");
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

    public bool IsMatch(string text)
    {
        return Regex.IsMatch(text, "^[0-9a-zA-Z-_]{0,10}$");
    }

    public void OnClick()
    {
        if (IsMatch(inputName.text))
        {
            // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
            PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions() { MaxPlayers = 6 }, TypedLobby.Default);
            button.interactable = false;
        }
        else
        {
            buttonText.text = "名前は半角英数字10文字以下";
        }
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        RankingData.ResetRanking();
        PhotonNetwork.LocalPlayer.NickName = inputName.text;
        SceneManager.LoadScene("GameScene");
    }

    // ルーム作成ができなかった時に呼ばれるコールバック
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("ルーム作成失敗");
        buttonText.text = "ルーム作成失敗";
        button.interactable = true;
    }

    // ルームに参加できなかった時に呼ばれるコールバック
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("ルーム参加失敗");
        buttonText.text = "現在接続できません";
    }

    // 新しくルームを作成したときに呼ばれるコールバック
    public override void OnCreatedRoom()
    {
        GameRoomProperty.ResetHashtable();
        GamePlayerProperty.ResetState();
    }

}
