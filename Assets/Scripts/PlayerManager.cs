using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{

    public Button readyButton;
    public Button backButton;
    public Button jumpButton;
    public Button leftButton;
    public Button rightButton;
    public Text countDown;
    public Text timerLabel;
    public Text stateText;

    private float countUp = 0.0f;
    private float autoDisconnectTime = 180.0f;
    private float autoDisconnectTime2 = 300.0f;

    public int startCountDown = 3;

    RectTransform rect;

    [DllImport("__Internal")]
    private static extern void CheckPlatform();

    void Start()
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        CheckPlatform();
#endif  
        rect = countDown.transform.GetComponent<RectTransform>();

        PhotonNetwork.IsMessageQueueRunning = true;

        if (PhotonNetwork.LocalPlayer.NickName != "admin")
        {
            PhotonNetwork.Instantiate("Player", new Vector2(-5, 0), Quaternion.identity);
            if (PhotonNetwork.LocalPlayer.NickName == "")
            {
                PhotonNetwork.LocalPlayer.NickName = "Player" + Random.Range(1000, 9999);
            }
            PhotonNetwork.CurrentRoom.SetPlayerState(PhotonNetwork.LocalPlayer.NickName, false);
        }
        PhotonNetwork.LocalPlayer.SetState(false);

        readyButton.onClick.AddListener(OnClickReadyButton);
        backButton.onClick.AddListener(OnClickBackButton);
        if (!GameManager.IsSmartPhone)
        {
            jumpButton.gameObject.SetActive(false);
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
        }
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
        Disconnect();
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
            if (count == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                readyButton.gameObject.SetActive(false);
                if (!GameManager.IsSmartPhone)
                {
                    backButton.gameObject.SetActive(false);
                }
                stateText.gameObject.SetActive(false);
                // 現在のサーバー時刻を、ゲームの開始時刻に設定する
                if (PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.HasCountDownTime())
                {
                    PhotonNetwork.CurrentRoom.SetCountDownTime(PhotonNetwork.ServerTimestamp);
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    StartCoroutine("WaitSeconds");
                }
            }
        }
    }

    IEnumerator WaitSeconds()
    {
        int cnt = 3;

        while (cnt > 0)
        {
            DOTween.Sequence()
                .OnStart(() => {
                    countDown.text = cnt.ToString();
                    countDown.gameObject.SetActive(true);
                })
                .AppendInterval(0.4f)
                .Append(rect.DOLocalMove(Vector2.zero, 0.6f))
                .Join(countDown.DOFade(0, 0.6f))
                .OnComplete(() => {
                    cnt--;
                    rect.localPosition = Vector2.up * 25f;
                    countDown.color = Color.black;
                    countDown.gameObject.SetActive(false);
                });
            yield return new WaitForSeconds(1f);

        }
        countDown.text = "START!!";

        DOTween.Sequence()
            .OnStart(() => {
                countDown.gameObject.SetActive(true);
            })
            .Append(rect.DOScale(Vector2.one * 1.8f, 2f))
            .Join(countDown.DOFade(0, 1f))
            .OnComplete(() => {
                countDown.gameObject.SetActive(false);
            });
    }

    void Disconnect()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("TitleScene");
    }

    void Update()
    {
        if (!PhotonNetwork.InRoom) { return; }
        if (!PhotonNetwork.CurrentRoom.TryGetCountDownTime(out int timestamp))
        {
            countUp += Time.deltaTime;
            if (PhotonNetwork.LocalPlayer.NickName != "admin" && countUp >= autoDisconnectTime)
            {
                Disconnect();
            }
            return;
        }
        int remainingTime = startCountDown - unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000;
        if (remainingTime > 0)
        {
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
            }
        }

        if (PhotonNetwork.CurrentRoom.TryGetCurrentTime(out string time))
        {
            timerLabel.text = time;
            // 0キーで切断
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Disconnect();
            }
            // 一定時間経過で自動的に切断
            if (float.Parse(time) >= autoDisconnectTime2)
            {
                Disconnect();
            }
        }
    }
}
