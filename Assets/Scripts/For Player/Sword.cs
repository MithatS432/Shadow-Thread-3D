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

    private PlayerMovement player;


    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GetComponentInParent<PlayerMovement>();
    }

    public void HitSword()
    {
        if (!canAttack)
            return;

        player.SetAttacking(true);

        if (Time.time - lastAttackTime > comboResetTime)
            attackIndex = 0;

        anim.SetTrigger("Attack" + (attackIndex + 1));

        if (swordHitClip != null)
            audioSource.PlayOneShot(swordHitClip);

        attackIndex = (attackIndex + 1) % 3;
        lastAttackTime = Time.time;
        canAttack = false;

        Invoke(nameof(ResetCooldown), attackCooldown);
    }


    void ResetCooldown()
    {
        canAttack = true;
    }
    public void AttackEnd()
    {
        player.SetAttacking(false);
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
