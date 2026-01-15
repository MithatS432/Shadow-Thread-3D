using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    public float health;

    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float detectionRange = 12f;
    public float loseInterestRange = 18f;

    [Header("Wander & Obstacle")]
    public float wanderRadius = 8f;
    public float obstacleCheckDistance = 2.5f;
    public LayerMask obstacleLayer;
    public float wanderChangeTime = 4f;
    private float wanderTimer;
    public Animator animator;
    public CharacterController controller;
    public Transform player;
    public AudioClip hurtClip;
    public AudioClip deathClip;

    public GameObject[] hurtEffects;
    private AudioSource audioSource;
    private int hurtEffectIndex;
    private float yVelocity;

    private Vector3 wanderTarget;

    private enum State { Walk, Run }
    private State currentState = State.Walk;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        wanderTimer = wanderChangeTime;


        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        PickNewWanderTarget();
    }

    void Update()
    {
        if (player == null)
            return;

        HandleGravity();

        float distance = Vector3.Distance(transform.position, player.position);
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            PickNewWanderTarget();
            wanderTimer = wanderChangeTime;
        }

        if (currentState == State.Walk)
        {
            Walk();

            if (distance <= detectionRange)
                Scare();
        }
        else if (currentState == State.Run)
        {
            RunAway();

            if (distance >= loseInterestRange)
                CalmDown();
        }
    }

    void Walk()
    {
        Vector3 dir = (wanderTarget - transform.position);
        dir.y = 0;

        if (dir.magnitude < 1f)
            PickNewWanderTarget();

        dir = AvoidObstacle(dir.normalized);

        RotateTowards(dir);

        controller.Move(dir * walkSpeed * Time.deltaTime + Vector3.up * yVelocity * Time.deltaTime);
    }

    void PickNewWanderTarget()
    {
        Vector2 random = Random.insideUnitCircle * wanderRadius;
        wanderTarget = transform.position + new Vector3(random.x, 0, random.y);
    }

    void RunAway()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        dir.y = 0;

        dir = AvoidObstacle(dir);

        RotateTowards(dir);

        controller.Move(dir * runSpeed * Time.deltaTime + Vector3.up * yVelocity * Time.deltaTime);
    }

    Vector3 AvoidObstacle(Vector3 dir)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, dir);

        if (Physics.Raycast(ray, obstacleCheckDistance, obstacleLayer))
        {
            Vector3 right = Vector3.Cross(Vector3.up, dir);
            float side = Random.value > 0.5f ? 1f : -1f;
            return (dir + right * side).normalized;

        }

        return dir;
    }

    void RotateTowards(Vector3 dir)
    {
        if (dir == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
    }

    void Scare()
    {
        if (currentState == State.Run)
            return;

        currentState = State.Run;
        animator.SetTrigger("Scare");
    }

    void CalmDown()
    {
        currentState = State.Walk;
        PickNewWanderTarget();
    }

    void HandleGravity()
    {
        if (controller.isGrounded)
            yVelocity = -1f;
        else
            yVelocity += Physics.gravity.y * Time.deltaTime;
    }




    #region  Health System
    public void GetDamage(float amount)
    {
        health -= amount;

        PlayHurtSound();
        PlayHurtEffect();
        Scare();

        if (health <= 0)
            Die();
    }

    void PlayHurtSound()
    {
        if (hurtClip != null && audioSource != null)
            audioSource.PlayOneShot(hurtClip);
    }

    void PlayHurtEffect()
    {
        if (hurtEffects.Length == 0)
            return;

        Instantiate(hurtEffects[hurtEffectIndex], transform.position, Quaternion.identity);

        hurtEffectIndex++;
        if (hurtEffectIndex >= hurtEffects.Length)
            hurtEffectIndex = 0;
    }

    void Die()
    {
        if (deathClip != null)
            AudioSource.PlayClipAtPoint(deathClip, transform.position);

        PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.AddKill();
        }

        Destroy(gameObject);
    }
    #endregion
}
