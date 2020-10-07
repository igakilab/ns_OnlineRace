using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class GameRoomProperty
{
    private const string KeyCountDownTime = "CountDownTime"; // カウントダウン開始時刻のキーの文字列
    private const string KeyStartTime = "StartTime"; // ゲーム開始時刻のキーの文字列

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
}