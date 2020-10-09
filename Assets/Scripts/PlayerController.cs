using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TextMeshPro nameLabel = default;

    [SerializeField]
    private TextMeshPro rankingLabel = default;

    public float speed = 0.5f;

    public new GameObject camera;
    public float xAdjust = 5f;
    public float yAdjust = 3f;

    public SpriteRenderer sr;

    private bool goal = false;

    void Start()
    {
        nameLabel.text = photonView.Owner.NickName;

        rankingLabel = GameObject.Find("RankingText").GetComponent<TextMeshPro>();

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
                transform.Translate(speed, 0f, 0f);
            }
            camera.transform.position = new Vector3(transform.position.x + xAdjust, camera.transform.position.y, camera.transform.position.z);

            if (!goal && transform.position.x > 20)
            {
                goal = true;
                photonView.RPC(nameof(WriteRanking), RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void FlipPlayer(bool state)
    {
        sr.flipX = state;
    }

    [PunRPC]
    public void WriteRanking()
    {
        rankingLabel.text = rankingLabel.text + photonView.Owner.NickName + "\n";
    }
}
