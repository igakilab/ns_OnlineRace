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

    private Animator anim = null;

    private Button backButton;
    private Button jumpButton;

    // ボタンを押したときtrue、離したときfalseになるフラグ
    private bool lButtonDownFlag = false;
    private bool rButtonDownFlag = false;

    public GroundCheck ground;

    public ReSpawnPoint reSpawnPosition;

    [SerializeField]
    private float speed = 10f;

    public GameObject camera;
    public float xAdjust = 5f;

    public SpriteRenderer sr;

    private float jumpPower = 500f;

    private bool goal = false;

    private Rigidbody2D rb;

    private bool isGround = false;
    private bool inoperable = false;

    private bool jumpFlag = false;
    private bool jump2Flag = false;
    private bool ballFlag = false;
    private bool upFlag = false;
    private bool up2Flag = false;

    void Start()
    {
        nameLabel.text = photonView.Owner.NickName;

        rankingLabel = GameObject.Find("RankingText").GetComponent<TextMeshPro>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();

        camera = GameObject.Find("Main Camera");

        anim = GetComponent<Animator>();

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
        if (collision.collider.tag == "Ball")
        {
            ballFlag = true;
            inoperable = true;
        }
        else if (collision.collider.tag == "Ground")
        {
            inoperable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Jump")
        {
            inoperable = true;
            jumpFlag = true;
        }
        else if (collision.tag == "Jump2")
        {
            inoperable = true;
            jump2Flag = true;
        }
        else if(collision.tag == "Up")
        {
            upFlag = true;
        }
        else if (collision.tag == "Up2")
        {
            up2Flag = true;
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
            if (Input.GetKeyDown(KeyCode.R))
            {
                rb.velocity = Vector2.zero;
                transform.position = reSpawnPosition.getPosition(transform.position);
            }
            //接地判定を得る
            isGround = ground.IsGround();
            anim.SetBool("fly", !isGround);
            float horizontalKey = Input.GetAxis("Horizontal");
            float xSpeed = 0.0f;
            if ((lButtonDownFlag || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !inoperable)
            {
                photonView.RPC(nameof(FlipPlayer), RpcTarget.All, true);
                anim.SetBool("run", true);
                if (transform.position.x > -8)
                {
                    xSpeed = -speed;
                }
            }
            else if ((rButtonDownFlag || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !inoperable)
            {
                photonView.RPC(nameof(FlipPlayer), RpcTarget.All, false);
                anim.SetBool("run", true);
                xSpeed = speed;
            }
            else
            {
                anim.SetBool("run", false);
            }
            // リファクタリング必須
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnClickJumpButton();
            }
            if (jumpFlag)
            {
                jumpFlag = false;
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(1000f, 500f));
            }
            else if (jump2Flag)
            {
                jump2Flag = false;
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(1000f, 300f));
            }
            else if (ballFlag)
            {
                ballFlag = false;
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(-400f, 600f));
            }
            else if (upFlag)
            {
                upFlag = false;
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0f, 250f));
            }
            else if (up2Flag)
            {
                up2Flag = false;
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0f, 1000f));
            }
            if (inoperable)
            {
                xSpeed = rb.velocity.x;
            }
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
            //Debug.Log(transform.position.y);
            camera.transform.position = new Vector3(Mathf.Clamp(transform.position.x + xAdjust, 0f, 280), camera.transform.position.y, camera.transform.position.z);

            //リスポ
            if (transform.position.y < -10)
            {
                rb.velocity = Vector2.zero;
                transform.position = reSpawnPosition.getPosition(transform.position);
            }

            //Debug.Log("X : " + transform.position.x + " Y : " + transform.position.y);
            if (!goal && transform.position.x > 270)
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
