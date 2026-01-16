using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    public enum AnimalState
    {
        Idle,
        Wander,
        Scared
    }

    private AnimalState currentState;

    private AnimalMovement movement;
    private AnimalSensor sensor;
    private Animator animator;

    private bool isScared;
    float idleTimer;
    float idleDuration;


    void Awake()
    {
        movement = GetComponent<AnimalMovement>();
        sensor = GetComponent<AnimalSensor>();
        animator = GetComponent<Animator>();
        currentState = AnimalState.Idle;

    }

    void Update()
    {
        switch (currentState)
        {
            case AnimalState.Idle:
                UpdateIdle();
                break;

            case AnimalState.Wander:
                UpdateWander();
                break;

            case AnimalState.Scared:
                UpdateScared();
                break;
        }
    }

    void UpdateIdle()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0)
            ChangeState(AnimalState.Wander);
    }
    void EnterIdle()
    {
        idleDuration = Random.Range(1.5f, 4f);
        idleTimer = idleDuration;
    }

    void UpdateWander()
    {
        if (sensor.ThreatDetected())
        {
            ChangeState(AnimalState.Scared);
            return;
        }

        movement.Wander();

        if (movement.ReachedTarget(movement.CurrentWanderTarget))
            ChangeState(AnimalState.Idle);
    }

    void UpdateScared()
    {
        movement.RunAway(sensor.ThreatPosition());

        if (sensor.LostThreat())
            ChangeState(AnimalState.Wander);
    }
    void ChangeState(AnimalState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case AnimalState.Idle:
                EnterIdle();
                break;

            case AnimalState.Wander:
                movement.PickNewTarget();
                break;

            case AnimalState.Scared:
                TriggerScare();
                break;
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
        ChangeState(AnimalState.Scared);
    }

}
