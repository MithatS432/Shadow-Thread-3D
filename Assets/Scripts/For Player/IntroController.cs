using UnityEngine;
using Unity.Cinemachine;

public class IntroController : MonoBehaviour
{
    public PlayerMovement player;
    public CinemachineCamera introCam;
    public CinemachineCamera playerCam;

    void Start()
    {
        player.canMove = false;

        introCam.Priority = 20;
        playerCam.Priority = 0;
    }

    public void OnIntroFinished()
    {
        introCam.Priority = 0;
        playerCam.Priority = 20;
        player.canMove = true;
    }
}
