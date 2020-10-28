using Photon.Pun;
using UnityEngine;

public class Watcher : MonoBehaviourPunCallbacks
{
    public GameObject mainCamera;
    public GameObject subCamera;
    public GameObject subCamera2;
    public GameObject subCamera3;

    public float cameraSpeed = 30f;
    private bool active = true;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("SubCamera");
        subCamera2 = GameObject.Find("SubCamera2");
        subCamera3 = GameObject.Find("SubCamera3");
        if (PhotonNetwork.LocalPlayer.NickName == "admin")
        {
            subCamera.SetActive(true);
            subCamera2.SetActive(true);
            subCamera3.SetActive(true);
        }
        else
        {
            subCamera.SetActive(false);
            subCamera2.SetActive(false);
            subCamera3.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.NickName == "admin")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (active)
                {
                    active = false;
                }
                else
                {
                    active = true;
                }
                mainCamera.SetActive(active);
                return;
            }
            if (!active) return;
            Vector3 cameraPosition = mainCamera.transform.position;
            if (Input.GetKey("left"))
            {
                cameraPosition.x -= cameraSpeed * Time.deltaTime;
            }
            else if (Input.GetKey("right"))
            {
                cameraPosition.x += cameraSpeed * Time.deltaTime;
            }
            mainCamera.transform.position = cameraPosition;
        }
    }
}
