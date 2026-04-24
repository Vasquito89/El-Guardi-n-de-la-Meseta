using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour , IDamageable
{
    [Header("Base Settings")]
    [SerializeField] protected Transform player;
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float detectionRange = 10f;
    [SerializeField] protected float attackRange = 2f;
    public float health = 50f;
    [SerializeField] protected float attackRate = 1.5f; // Tiempo entre ataques (ajusta según tu sonido)
    private float nextAttackTime = 0f; // Marcador de tiempo para el siguiente ataque

    //public int lifeEnemy { get; set; }

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected SoundEnemyController soundController;
    protected GuanacoController playerController;
    protected GameManager gameManager;


    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        soundController = GetComponentInChildren<SoundEnemyController>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        playerController = FindObjectOfType<GuanacoController>();
        if (playerController == null)
            Debug.LogError($"¡Atención! {gameObject.name} no encontró al GuanacoController.");
        gameManager = FindObjectOfType<GameManager>();


        // Disable enemy until the LevelManager activates it
        //gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackRate; // Actualiza el tiempo para el próximo ataque
            }
            else
            {
                // Mientras espera el cooldown, que se quede quieto
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else if (distance <= detectionRange)
        {
            Chase();
        }
        else
        {
            StayStill();
        }
    }

    protected void Chase()
    {
        animator.SetBool("isWalking", true);
        float directionX = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(directionX * speed, rb.velocity.y);
        spriteRenderer.flipX = (player.position.x > transform.position.x);
        soundController.PlayWalk();
    }

    protected void StayStill()
    {
        animator.SetBool("isWalking", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
        soundController.StopWalk();
    }

    protected abstract void Attack();

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        animator.SetTrigger("die");
        rb.velocity = Vector2.zero; //
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        if (gameManager != null)
        {
            // Pasamos un pequeño delay o llamamos después de destruir
            Destroy(gameObject);
            // Usamos Invoke para dar un frame de margen a que Unity limpie la lista de objetos
            gameManager.Invoke("CheckGameStatus", 0.1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
