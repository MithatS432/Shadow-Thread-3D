using UnityEngine;

public class Sword : MonoBehaviour
{
    private Animator anim;

    private AudioSource audioSource;
    public AudioClip swordHitClip;


    [Header("Combo Settings")]
    public float attackCooldown = 0.4f;
    public float comboResetTime = 1.2f;

    private int attackIndex = 0;
    private float lastAttackTime;
    private bool canAttack = true;
    [SerializeField] private float damage = 30f;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void HitSword()
    {
        if (!canAttack)
            return;

        if (Time.time - lastAttackTime > comboResetTime)
        {
            attackIndex = 0;
        }

        switch (attackIndex)
        {
            case 0:
                anim.SetTrigger("Attack1");
                break;
            case 1:
                anim.SetTrigger("Attack2");
                break;
            case 2:
                anim.SetTrigger("Attack3");
                break;
        }

        if (swordHitClip != null)
            audioSource.PlayOneShot(swordHitClip);

        attackIndex++;
        if (attackIndex > 2)
            attackIndex = 0;

        lastAttackTime = Time.time;
        canAttack = false;

        Invoke(nameof(ResetCooldown), attackCooldown);
    }

    void ResetCooldown()
    {
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canAttack) return;

        if (other.gameObject.CompareTag("Animal"))
        {
            AnimalStats animalStats = other.gameObject.GetComponent<AnimalStats>();
            if (animalStats != null)
            {
                animalStats.GetDamage(damage);
            }
        }
    }
}
