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
        }
    }

    void Update()
    {
        
    }
}
