using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{

    public Button readyButton;
    public Button backButton;
    public Text countDownLabel;
    public Text timerLabel;
    public Text stateText;

    public int countDown = 3;

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        PhotonNetwork.Instantiate("Player", new Vector2(-5, -3), Quaternion.identity);

        PhotonNetwork.CurrentRoom.SetPlayerState(PhotonNetwork.LocalPlayer.NickName, false);

        PhotonNetwork.LocalPlayer.SetState(false);

        readyButton.onClick.AddListener(OnClickReadyButton);
        backButton.onClick.AddListener(OnClickBackButton);
    }

    public void OnClickReadyButton()
    {
        if (!PhotonNetwork.LocalPlayer.GetState())
        {
            PhotonNetwork.LocalPlayer.SetState(true);
            PhotonNetwork.CurrentRoom.SetPlayerState(PhotonNetwork.LocalPlayer.NickName, true);
            readyButton.interactable = false;
        }
    }

    public void OnClickBackButton()
    {
        backButton.interactable = false;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("TitleScene");
    }

    // ルームのカスタムプロパティが更新された時に呼ばれるコールバック
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.HasPlayerState())
        {
            stateText.text = PhotonNetwork.CurrentRoom.TryGetPlayerState(out string state);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerStateExits(otherPlayer.NickName))
        {
            PhotonNetwork.CurrentRoom.DeletePlayerState(otherPlayer.NickName);
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
                backButton.gameObject.SetActive(false);
                // 現在のサーバー時刻を、ゲームの開始時刻に設定する
                if (PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.HasCountDownTime())
                {
                    PhotonNetwork.CurrentRoom.SetCountDownTime(PhotonNetwork.ServerTimestamp);
                    PhotonNetwork.CurrentRoom.IsOpen = false;
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

        if (PhotonNetwork.CurrentRoom.TryGetCurrentTime(out string time))
        {
            timerLabel.text = time;
        }
    }
}
