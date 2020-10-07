using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TextMeshPro nameLabel = default;

    void Start()
    {
        nameLabel.text = photonView.Owner.NickName;
    }

    void Update()
    {
    }
}
