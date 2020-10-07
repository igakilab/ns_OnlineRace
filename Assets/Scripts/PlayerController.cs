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
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(0f, 0.05f, 0f);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(0f, -0.05f, 0f);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-0.05f, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(0.05f, 0f, 0f);
            }
        }
    }
}
