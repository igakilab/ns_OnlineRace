using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TextMeshPro nameLabel = default;

    public new GameObject camera;
    public float xAdjust = 5f;
    public float yAdjust = 3f;

    void Start()
    {
        nameLabel.text = photonView.Owner.NickName;

        camera = Camera.main.gameObject;
    }

    void Update()
    {
        // ゲームがスタートするまで動けない
        if (!PhotonNetwork.CurrentRoom.HasStartTime()) { return; }
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
            camera.transform.position = new Vector3(transform.position.x + xAdjust, transform.position.y + yAdjust, camera.transform.position.z);
        }
    }
}
