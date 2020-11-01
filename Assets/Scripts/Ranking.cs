using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string GetRanking();

    [SerializeField]
    private TextMeshPro ranking = default;

    void Start()
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        GetRanking();
#endif
    }

    public void UpdateRanking(string rank)
    {
        ranking.text = rank;
    }

}
