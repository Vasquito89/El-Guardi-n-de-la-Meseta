using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Ajustes de Combate")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float speed = 10f;
    public string targetTag; // "Enemy" para Guanaco, "Player" para Hunter

    [Header("Optimización")]
    [SerializeField] private float lifeTime = 2f;

    void Start()
    {       
        Destroy(gameObject, lifeTime); // Esta función programa la destrucción del objeto apenas aparece en escena
    }

    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject); 
        }
    }
}