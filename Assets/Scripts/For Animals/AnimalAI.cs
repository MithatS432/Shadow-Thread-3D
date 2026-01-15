using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    private AnimalMovement movement;
    private AnimalSensor sensor;
    private Animator animator;

    private bool isScared;

    void Awake()
    {
        movement = GetComponent<AnimalMovement>();
        sensor = GetComponent<AnimalSensor>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (sensor.ThreatDetected())
        {
            TriggerScare();
            movement.RunAway(sensor.ThreatPosition());
        }
        else
        {
            isScared = false;
            movement.Wander();
        }
    }

    void TriggerScare()
    {
        if (isScared)
            return;

        isScared = true;
        animator.SetTrigger("Scare");
    }

    public void ForceScare()
    {
        TriggerScare();
    }
}
