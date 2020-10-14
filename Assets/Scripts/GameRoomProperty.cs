using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class GameRoomProperty
{
    private const string KeyCountDownTime = "CountDownTime"; // カウントダウン開始時刻のキーの文字列
    private const string KeyStartTime = "StartTime"; // ゲーム開始時刻のキーの文字列
    private const string KeyPlayerState = "PlayerState"; // プレイヤーの準備状況のキーの文字列

    private static Hashtable timeHashtable = new Hashtable();
    private static Hashtable stateHashtable = new Hashtable();

    public static bool HasCountDownTime(this Room room)
    {
        return room.CustomProperties.ContainsKey(KeyCountDownTime);
    }

    public static bool TryGetCountDownTime(this Room room, out int timestamp)
    {
        if (room.CustomProperties[KeyCountDownTime] is int value)
        {
            timestamp = value;
            return true;
        }
        timestamp = 0;
        return false;
    }

    public static void SetCountDownTime(this Room room, int timestamp)
    {
        timeHashtable[KeyCountDownTime] = timestamp;

        room.SetCustomProperties(timeHashtable);
    }

    public static bool HasStartTime(this Room room)
    {
        return room.CustomProperties.ContainsKey(KeyStartTime);
    }

    public static bool TryGetStartTime(this Room room, out int timestamp)
    {
        if (room.CustomProperties[KeyStartTime] is int value)
        {
            timestamp = value;
            return true;
        }
        timestamp = 0;
        return false;
    }

    public static bool TryGetCurrentTime(this Room room, out string time)
    {
        if (room.TryGetStartTime(out int timestamp)) {
            time = Mathf.Max(unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f).ToString("f2");
            return true;
        }
        time = "0.0";
        return false;
    }

    public static void SetStartTime(this Room room, int timestamp)
    {
        timeHashtable[KeyStartTime] = timestamp;

        room.SetCustomProperties(timeHashtable);
    }

    public static bool HasPlayerState(this Room room)
    {
        return room.CustomProperties.ContainsKey(KeyPlayerState);
    }

    public static bool HasPlayerState(this Hashtable hashtable)
    {
        return hashtable[KeyPlayerState] is Dictionary<string, bool>;
    }

    public static string TryGetPlayerState(this Room room, out string state)
    {
        state = "";
        if (room.CustomProperties[KeyPlayerState] is Dictionary<string, bool> table)
        {
            foreach (KeyValuePair<string, bool> item in table)
            {
                state = state + item.Key + (item.Value ? " 準備完了" : " 準備中") + "\n";
            }
            return state;
        }
        return state;
    }

    public static Dictionary<string, bool> TryGetPlayerState(this Room room)
    {
        return (room.CustomProperties[KeyPlayerState] is Dictionary<string, bool> value) ? value : new Dictionary<string, bool>();
    }

    public static void SetPlayerState(this Room room, string player, bool state)
    {
        Dictionary<string, bool> table = TryGetPlayerState(room);
        table[player] = state;
        stateHashtable[KeyPlayerState] = table;
        room.SetCustomProperties(stateHashtable);
    }

    public static bool PlayerStateExits(this Room room, string player)
    {
        if (HasPlayerState(room))
        {
            Dictionary<string, bool> table = TryGetPlayerState(room);
            return table.ContainsKey(player);
        }
        return false;
    }

    public static void DeletePlayerState(this Room room, string player)
    {
        Dictionary<string, bool> table = TryGetPlayerState(room);
        table.Remove(player);
        stateHashtable[KeyPlayerState] = table;
        room.SetCustomProperties(stateHashtable);
    }

    public static void ResetHashtable()
    {
        timeHashtable.Clear();
        stateHashtable.Clear();
    }
}