using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifeTime = 5f;
    public float damage = 25f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Animal"))
        {
            AnimalStats animalStats = other.gameObject.GetComponent<AnimalStats>();
            if (animalStats != null)
            {
                animalStats.GetDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
