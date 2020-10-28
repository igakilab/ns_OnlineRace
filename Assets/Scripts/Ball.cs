using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviourPunCallbacks
{
    private float countup = 0.0f;

    //タイムリミット
    private float timeLimit = 1.5f;

    private Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Fire()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = new Vector2(61f, -3.3f);
        rb.AddForce(new Vector2(-1000f, 0f));
    }

    private void Update()
    {
        countup += Time.deltaTime;
        if (countup >= timeLimit)
        {
            countup = 0f;
            Fire();
        }
    }

}