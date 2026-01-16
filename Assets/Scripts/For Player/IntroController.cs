using UnityEngine;
using Unity.Cinemachine;

public class IntroController : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject playerRoot;

    public CinemachineCamera introCam;
    public CinemachineCamera playerCam;

    void Start()
    {
        playerRoot.SetActive(false);

        introCam.Priority = 20;
        playerCam.Priority = 0;
    }

    public void OnIntroFinished()
    {
        Debug.Log("INTRO BITTI");

        playerRoot.SetActive(true);
        player.canMove = true;

        introCam.Priority = 0;
        playerCam.Priority = 20;
    }
}
