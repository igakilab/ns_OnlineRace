using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputName : MonoBehaviour
{

    private TouchScreenKeyboard keyboard;

    void Start()
    {

    }

    public void SelectButton()
    {
        this.keyboard = TouchScreenKeyboard.Open("キーボードに最初に入れておくテキスト", TouchScreenKeyboardType.Default);
        //後から変更も可能
        this.keyboard.text = "キーボードに入れるテキスト";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
