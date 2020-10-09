using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TextMeshPro nameLabel = default;

    public float speed = 0.5f;

    public new GameObject camera;
    public float xAdjust = 5f;
    public float yAdjust = 3f;

    public SpriteRenderer sr;

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
                transform.Translate(0f, speed, 0f);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(0f, -speed, 0f);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                photonView.RPC(nameof(FlipPlayer), RpcTarget.All, false);
                transform.Translate(-speed, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                photonView.RPC(nameof(FlipPlayer), RpcTarget.All, true);
                sr.flipX = true;
                transform.Translate(speed, 0f, 0f);
            }
            camera.transform.position = new Vector3(transform.position.x + xAdjust, camera.transform.position.y, camera.transform.position.z);

            if (transform.position.x > 20)
            {
                Debug.Log("ゴール！！");
            }
        }
    }

    [PunRPC]
    public void FlipPlayer(bool state)
    {
        sr.flipX = state;
    }
}
