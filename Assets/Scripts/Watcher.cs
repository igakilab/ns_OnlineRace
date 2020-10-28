using Photon.Pun;
using UnityEngine;

public class Watcher : MonoBehaviourPunCallbacks
{
    public GameObject mainCamera;
    public GameObject subCamera;
    public GameObject subCamera2;

    public float cameraSpeed = 30f;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("SubCamera");
        subCamera2 = GameObject.Find("SubCamera2");
        if (PhotonNetwork.LocalPlayer.NickName == "admin")
        {
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
            subCamera2.SetActive(true);
        }
        else
        {
            subCamera.SetActive(false);
            subCamera2.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //監視
        /*
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
        */
    }
}
