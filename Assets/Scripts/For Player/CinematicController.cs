using UnityEngine;
using UnityEngine.Playables;

public class CinematicController : MonoBehaviour
{
    public PlayerMovement player;

    public void OnCinematicStart()
    {
        player.canMove = false;
        player.enabled = false;
    }


    public void OnCinematicEnd()
    {
        player.canMove = true;
        player.enabled = true;
    }
}
