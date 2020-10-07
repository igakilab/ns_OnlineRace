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

    public int countDown = 5;

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Player", v, Quaternion.identity);

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

    // プレイヤーのカスタムプロパティが更新された時に呼ばれるコールバック
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        // 準備完了ボタンが押されたら
        if (PhotonNetwork.IsMasterClient && changedProps.TryGetState(out bool value))
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
                // 現在のサーバー時刻を、ゲームの開始時刻に設定する
                if (!PhotonNetwork.CurrentRoom.HasCountDownTime())
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
