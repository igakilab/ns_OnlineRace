using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TextMeshPro nameLabel = default;

    [SerializeField]
    private TextMeshPro rankingLabel = default;

    public Button backButton;

    public GroundCheck ground;

    public float speed = 10f;

    public new GameObject camera;
    public float xAdjust = 5f;
    public float yAdjust = 3f;

    public SpriteRenderer sr;

    private float jumpPower = 700f;

    private bool goal = false;

    private Rigidbody2D rb;

    private bool isGround = false;

    void Start()
    {
        nameLabel.text = photonView.Owner.NickName;

        rankingLabel = GameObject.Find("RankingText").GetComponent<TextMeshPro>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();

        camera = Camera.main.gameObject;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ゲームがスタートするまで動けない
        if (!PhotonNetwork.CurrentRoom.HasStartTime() || goal) { return; }
        if (photonView.IsMine)
        {
            //接地判定を得る
            isGround = ground.IsGround();

            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                rb.AddForce(Vector2.up * jumpPower);
            }
            transform.Translate(speed * Time.deltaTime, 0, 0);
            camera.transform.position = new Vector3(transform.position.x + xAdjust, camera.transform.position.y, camera.transform.position.z);

            if (!goal && transform.position.x > 20)
            {
                goal = true;
                if (PhotonNetwork.CurrentRoom.TryGetCurrentTime(out string time))
                {
                    photonView.RPC(nameof(WriteRanking), RpcTarget.AllViaServer, time);
                }
                backButton.gameObject.SetActive(true);
            }
        }
    }

    [PunRPC]
    public void WriteRanking(string time)
    {
        RankingData.SetRanking(photonView.Owner.NickName, time);
        rankingLabel.text = RankingData.GetRanking();
    }
}
