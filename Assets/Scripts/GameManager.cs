using UnityEngine;

public class GameManager : MonoBehaviour
{
    //スマホかどうかを保存しておく
    public static bool IsSmartPhone { get; private set; } = false;

    //JavaScriptから呼び出すメソッド
    public void setSmartPhoneMode()
    {
        IsSmartPhone = true;
    }
}