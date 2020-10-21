using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TextMeshPro nameLabel = default;

    [SerializeField]
    private TextMeshPro rankingLabel = default;

    private Button backButton;
    private Button jumpButton;

    // ボタンを押したときtrue、離したときfalseになるフラグ
    private bool lButtonDownFlag = false;
    private bool rButtonDownFlag = false;

    public GroundCheck ground;

    [SerializeField]
    private float speed = 10f;

    public new GameObject camera;
    public float xAdjust = 5f;
    public float yAdjust = 3f;

    public SpriteRenderer sr;

    private float jumpPower = 500f;

    private bool goal = false;

    private Rigidbody2D rb;

    private bool isGround = false;
    private bool inoperable = false;

    void Start()
    {
        nameLabel.text = photonView.Owner.NickName;

        rankingLabel = GameObject.Find("RankingText").GetComponent<TextMeshPro>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();

        camera = Camera.main.gameObject;

        rb = GetComponent<Rigidbody2D>();

        if (GameManager.IsSmartPhone)
        {
            jumpButton = GameObject.Find("JumpButton").GetComponent<Button>();
            jumpButton.onClick.AddListener(OnClickJumpButton);

            EventTrigger trigger = GameObject.Find("LeftButton").AddComponent<EventTrigger>();
            trigger.triggers = new List<EventTrigger.Entry>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(call => { lButtonDownFlag = true; });
            trigger.triggers.Add(entry);

            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerUp;
            entry2.callback.AddListener(call => { lButtonDownFlag = false; });
            trigger.triggers.Add(entry2);

            EventTrigger trigger2 = GameObject.Find("RightButton").AddComponent<EventTrigger>();
            trigger2.triggers = new List<EventTrigger.Entry>();

            EventTrigger.Entry entry3 = new EventTrigger.Entry();
            entry3.eventID = EventTriggerType.PointerDown;
            entry3.callback.AddListener(call => { rButtonDownFlag = true; });
            trigger2.triggers.Add(entry3);

            EventTrigger.Entry entry4 = new EventTrigger.Entry();
            entry4.eventID = EventTriggerType.PointerUp;
            entry4.callback.AddListener(call => { rButtonDownFlag = false; });
            trigger2.triggers.Add(entry4);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            rb.AddForce(new Vector2(-400f, 600f));
            inoperable = true;
        }
        else if (collision.collider.tag == "Ground")
        {
            inoperable = false;
        }
    }

    public void OnClickJumpButton()
    {
        if (isGround)
        {
            ground.SetGround(false);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpPower);
        }
    }

    void Update()
    {
        // ゲームがスタートするまで動けない
        if (!PhotonNetwork.CurrentRoom.HasStartTime() || goal) { return; }
        if (photonView.IsMine)
        {
            //接地判定を得る
            isGround = ground.IsGround();
            float horizontalKey = Input.GetAxis("Horizontal");
            float xSpeed = 0.0f;
            if ((lButtonDownFlag || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !inoperable)
            {
                photonView.RPC(nameof(FlipPlayer), RpcTarget.All, false);
                if (transform.position.x > -8)
                {
                    //transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
                    xSpeed = -speed;
                }
            }
            else if ((rButtonDownFlag || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !inoperable)
            {
                photonView.RPC(nameof(FlipPlayer), RpcTarget.All, true);
                //transform.Translate(new Vector2(speed * Time.deltaTime, 0));
                xSpeed = speed;
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                ground.SetGround(false);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpPower);
            }
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
            //Debug.Log(transform.position.x + xAdjust);
            camera.transform.position = new Vector3(Mathf.Clamp(transform.position.x + xAdjust, 0f, 180f), camera.transform.position.y, camera.transform.position.z);

            //落ちたらリスポ
            if (transform.position.y < -10)
            {
                transform.position = Vector2.zero;
            }

            //Debug.Log("X : " + transform.position.x + " Y : " + transform.position.y);
            if (!goal && transform.position.x > 170)
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
    public void FlipPlayer(bool state)
    {
        sr.flipX = state;
    }

    [PunRPC]
    public void WriteRanking(string time)
    {
        RankingData.SetRanking(photonView.Owner.NickName, time);
        rankingLabel.text = RankingData.GetRanking();
    }
}
