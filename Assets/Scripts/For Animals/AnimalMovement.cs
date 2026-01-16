using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    private CharacterController controller;
    private AnimalStats stats;
    private float yVelocity;
    private Vector3 wanderTarget;
    public Vector3 CurrentWanderTarget => wanderTarget;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        stats = GetComponent<AnimalStats>();
        PickNewWanderTarget();
    }

    public void Wander()
    {
        Vector3 dir = (wanderTarget - transform.position);
        dir.y = 0;
        dir = AvoidObstacle(dir.normalized);

        Move(dir, stats.WalkSpeed);

    }

    public bool ReachedTarget(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) < 1f;
    }

    public void RunAway(Vector3 threatPos)
    {
        Vector3 dir = (transform.position - threatPos);
        dir.y = 0;

        dir = AvoidObstacle(dir.normalized);

        Move(dir, stats.RunSpeed);
    }
    public void PickNewTarget()
    {
        PickNewWanderTarget();
    }


    void Move(Vector3 dir, float speed)
    {
        ApplyGravity();
        controller.Move(dir * speed * Time.deltaTime + Vector3.up * yVelocity * Time.deltaTime);
        Rotate(dir);
    }

    void PickNewWanderTarget()
    {
        Vector2 r = Random.insideUnitCircle * stats.WanderRadius;
        wanderTarget = transform.position + new Vector3(r.x, 0, r.y);
    }

    void Rotate(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 10f
        );
    }

    void ApplyGravity()
    {
        if (controller.isGrounded)
            yVelocity = -1f;
        else
            yVelocity += Physics.gravity.y * Time.deltaTime;
    }
    Vector3 AvoidObstacle(Vector3 dir)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, dir);

        if (Physics.Raycast(ray, 2.5f))
        {
            Vector3 right = Vector3.Cross(Vector3.up, dir);
            float side = Random.value > 0.5f ? 1f : -1f;
            return (dir + right * side).normalized;
        }

        return dir;
    }

}
