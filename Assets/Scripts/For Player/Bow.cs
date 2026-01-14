using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public Camera playerCamera;
    public float shootForce = 35f;
    public AudioClip arrowShootSound;

    public float fireRate = 0.5f;
    private float lastShootTime = 0f;

    public bool Shoot()
    {
        if (Time.time - lastShootTime < fireRate)
            return false;

        lastShootTime = Time.time;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000f);

        Vector3 direction = (targetPoint - arrowSpawnPoint.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation *= Quaternion.Euler(90f, 0f, 0f);

        GameObject arrow = Instantiate(
            arrowPrefab,
            arrowSpawnPoint.position,
            rotation
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * shootForce;

        if (arrowShootSound != null)
            AudioSource.PlayClipAtPoint(arrowShootSound, arrowSpawnPoint.position);

        return true;
    }


}
