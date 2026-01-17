using UnityEngine;
using Unity.Cinemachine;

public class CinematicController : MonoBehaviour
{
    public PlayerMovement player;
    public CinemachineCamera cinematicCam;

    public void OnCinematicStart()
    {
        player.canMove = false;
        player.enabled = false;

        cinematicCam.enabled = true;
    }

    public void OnCinematicEnd()
    {
        cinematicCam.enabled = false;

        player.enabled = true;
        player.canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
