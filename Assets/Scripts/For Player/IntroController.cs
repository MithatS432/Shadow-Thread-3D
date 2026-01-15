using UnityEngine;
using Unity.Cinemachine;

public class IntroController : MonoBehaviour
{
    public PlayerMovement player;
    public CinemachineCamera introCam;
    public CinemachineCamera playerCam;

    public void OnIntroFinished()
    {
        introCam.Priority = 0;
        playerCam.Priority = 20;
        player.canMove = true;
    }
}
