using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class GamePlayerProperty
{
    private const string StateKey = "Ready"; // 準備状態のキーの文字列

    private static Hashtable hashtable = new Hashtable();

    // （Hashtableに）プレイヤーの準備状態が設定されていれば取得する
    public static bool TryGetState(this Hashtable hashtable, out bool state)
    {
        return state = (hashtable[StateKey] is bool value) ? value : false;
    }

    // プレイヤーの準備状態を取得する
    public static bool GetState(this Player player)
    {
        return player.CustomProperties.TryGetState(out bool state);
    }

    public static void SetState(this Player player, bool state)
    {
        hashtable[StateKey] = state;
        player.SetCustomProperties(hashtable);
    }
}