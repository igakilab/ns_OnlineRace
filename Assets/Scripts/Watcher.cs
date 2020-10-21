using Photon.Pun;
using UnityEngine;

public class Watcher : MonoBehaviourPunCallbacks
{
    public new GameObject camera;

    public float cameraSpeed = 30f;

    void Start()
    {
        camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //監視
        if (PhotonNetwork.LocalPlayer.NickName == "admin")
        {
            Vector3 cameraPosition = camera.transform.position;
            if (Input.GetKey("left"))
            {
                cameraPosition.x -= cameraSpeed * Time.deltaTime;
            }
            else if (Input.GetKey("right"))
            {
                cameraPosition.x += cameraSpeed * Time.deltaTime;
            }
            camera.transform.position = cameraPosition;
            return;
        }
    }
}
