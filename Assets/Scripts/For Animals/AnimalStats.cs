using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    public float health;
    void Start()
    {

    }

    void Update()
    {

    }
    public void GetDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                player.AddKill();
            }
        }
    }
}
