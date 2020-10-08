using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{

    public Button readyButton;
    public Text countDownLabel;
    public Text timerLabel;
    public Text stateText;

    public int countDown = 3;

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        PhotonNetwork.Instantiate("Player", new Vector2(-5, -3), Quaternion.identity);

        PhotonNetwork.CurrentRoom.SetKeyPlayerState(PhotonNetwork.LocalPlayer.NickName);
        //stateText.text = PhotonNetwork.CurrentRoom.TryGetKeyPlayerState(out string state) + PhotonNetwork.LocalPlayer.NickName;

        PhotonNetwork.LocalPlayer.SetState(false);

        readyButton.onClick.AddListener(OnClickReadyButton);
    }

    public void OnClickReadyButton()
    {
        if (!PhotonNetwork.LocalPlayer.GetState())
        {
            PhotonNetwork.LocalPlayer.SetState(true);
            readyButton.interactable = false;
        }
    }

    // ルームのカスタムプロパティが更新された時に呼ばれるコールバック
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            stateText.text = PhotonNetwork.CurrentRoom.TryGetKeyPlayerState(out string state);
        }
    }

    // プレイヤーのカスタムプロパティが更新された時に呼ばれるコールバック
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        // 準備完了ボタンが押されたら
        if (changedProps.TryGetState(out bool value))
        {
            int count = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.GetState())
                {
                    count++;
                }
            }
            Debug.Log($"準備完了 {count}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
            if (count == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                readyButton.gameObject.SetActive(false);
                // 現在のサーバー時刻を、ゲームの開始時刻に設定する
                if (PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.HasCountDownTime())
                {
                    PhotonNetwork.CurrentRoom.SetCountDownTime(PhotonNetwork.ServerTimestamp);
                }
            }
        }
    }

    void Update()
    {
        if (!PhotonNetwork.InRoom) { return; }
        if (!PhotonNetwork.CurrentRoom.TryGetCountDownTime(out int timestamp)) { return; }

        int remainingTime = countDown - unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000;
        if (remainingTime > 0)
        {
            countDownLabel.text = remainingTime.ToString();
            return;
        }
        else
        {
            if (remainingTime > -3)
            {
                if (PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.HasStartTime())
                {
                    PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
                }
                countDownLabel.text = "START!!!";
            }
            else
            {
                countDownLabel.text = "";
            }
        }
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp2)) { return; }
        timerLabel.text = Mathf.Max(unchecked(PhotonNetwork.ServerTimestamp - timestamp2) / 1000f).ToString("f2");
    }
}
