using UnityEngine;

public class AnimalEffects : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip hurtClip;
    public AudioClip deathClip;

    [Header("Hurt Effects")]
    public GameObject[] hurtEffects;

    private int effectIndex;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHurt()
    {
        PlayHurtSound();
        PlayHurtEffect();
    }

    public void PlayDeath()
    {
        if (deathClip != null)
            AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }

    void PlayHurtSound()
    {
        if (hurtClip != null && audioSource != null)
            audioSource.PlayOneShot(hurtClip);
    }

    void PlayHurtEffect()
    {
        if (hurtEffects == null || hurtEffects.Length == 0)
            return;

        if (VFXManager.Instance != null && !VFXManager.Instance.CanPlayVFX())
            return;

        GameObject effect = Instantiate(
            hurtEffects[effectIndex],
            transform.position,
            Quaternion.identity
        );
        Destroy(effect, 1.5f);

        effectIndex++;
        if (effectIndex >= hurtEffects.Length)
            effectIndex = 0;
    }

}
