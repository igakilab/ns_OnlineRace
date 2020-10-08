using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class GameRoomProperty
{
    private const string KeyCountDownTime = "CountDownTime"; // カウントダウン開始時刻のキーの文字列
    private const string KeyStartTime = "StartTime"; // ゲーム開始時刻のキーの文字列
    private const string KeyPlayerState = "PlayerState"; // プレイヤーの準備状況のキーの文字列

    private static Hashtable hashtable = new Hashtable();

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
        hashtable[KeyCountDownTime] = timestamp;

        room.SetCustomProperties(hashtable);
        hashtable.Clear();
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

    public static void SetStartTime(this Room room, int timestamp)
    {
        hashtable[KeyStartTime] = timestamp;

        room.SetCustomProperties(hashtable);
        hashtable.Clear();
    }

    public static bool HasPlayerState(this Room room)
    {
        return room.CustomProperties.ContainsKey(KeyPlayerState);
    }

    public static string TryGetKeyPlayerState(this Room room, out string state)
    {
        if (room.CustomProperties[KeyPlayerState] is string value)
        {
            state = value;
            return state;
        }
        state = "";
        return state;
    }

    public static void SetKeyPlayerState(this Room room, string state)
    {
        hashtable[KeyPlayerState] = TryGetKeyPlayerState(room, out string val) + state + "\n";

        room.SetCustomProperties(hashtable);
        hashtable.Clear();
    }
}