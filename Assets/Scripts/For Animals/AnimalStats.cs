using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    public AnimalTypeData typeData;
    private AnimalAI ai;


    [Header("Health")]
    public float health = 100f;

    public float WalkSpeed => typeData.walkSpeed;
    public float RunSpeed => typeData.runSpeed;
    public float DetectionRange => typeData.detectionRange;
    public float LoseRange => typeData.loseInterestRange;
    public float WanderRadius => typeData.wanderRadius;

    private AnimalEffects effects;

    void Awake()
    {
        effects = GetComponent<AnimalEffects>();
        ai = GetComponent<AnimalAI>();
    }

    public void GetDamage(float amount)
    {
        health -= amount;

        effects?.PlayHurt();
        ai?.ForceScare();

        if (health <= 0)
            Die();
    }

    void Die()
    {
        effects?.PlayDeath();
        Destroy(gameObject);
    }
}
