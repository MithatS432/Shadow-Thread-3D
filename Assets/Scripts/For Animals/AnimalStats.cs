using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    public float health;
    public GameObject[] hurtEffects;
    private int hurtEffectIndex = 0;
    public AudioClip hurtClip;
    private AudioSource audioSource;
    public float hurtSoundCooldown = 0.15f;
    private float lastHurtSoundTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void GetDamage(float amount)
    {
        health -= amount;

        if (Time.time - lastHurtSoundTime > hurtSoundCooldown)
        {
            audioSource.PlayOneShot(hurtClip);
            lastHurtSoundTime = Time.time;
        }
        PlayHurtEffect();

        if (health <= 0)
        {
            Destroy(gameObject);

            PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                player.AddKill();
            }
        }
    }

    void PlayHurtEffect()
    {
        if (hurtEffects.Length == 0)
            return;

        GameObject effect = Instantiate(
            hurtEffects[hurtEffectIndex],
            transform.position,
            Quaternion.identity
        );

        Destroy(effect, 1.5f);

        hurtEffectIndex++;
        if (hurtEffectIndex >= hurtEffects.Length)
            hurtEffectIndex = 0;
    }
}
