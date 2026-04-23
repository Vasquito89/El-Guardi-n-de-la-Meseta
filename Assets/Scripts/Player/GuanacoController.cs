using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuanacoController : MonoBehaviour,IDamageable
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Disparo")]
    [SerializeField] private GameObject salivaPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackDamage = 15f;

    // Variables para la UI (El GameManager leerá estas)
    public float lifeGuanaco = 100f;
    public float currentAmmo = 5f;

    private bool isOnFood = false;
    private GameObject currentFood;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private float horizontalInput;
    private EnemyBase currentEnemy;

    void Awake()
    {
        lifeGuanaco = 100f;
        currentAmmo = 5f;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        
    }

    void Update()
    {
        // Leer input de flechas o WASD
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // ENVIAR DATOS AL ANIMATOR
        // Usamos Mathf.Abs para que el valor siempre sea positivo (1 si vas a derecha o izquierda)
        // El parámetro en Unity debe llamarse "velocidadHorizontal"
        if (animator != null)
        {
            animator.SetFloat("velocidadHorizontal", Mathf.Abs(horizontalInput));
        }

        // Saltar si presiona Espacio y está tocando el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 4. Atacar (Flecha Arriba)
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentAmmo > 0)
        {
            PerformAttack();
        }

        // 5. Comer (Flecha Abajo)
        if (isOnFood && Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(PerformEat());
        }

        // Girar el sprite según la dirección
        if (horizontalInput > 0 && !facingRight) Spin();
        else if (horizontalInput < 0 && facingRight) Spin();

    }

    void FixedUpdate()
    {
        // Aplicar movimiento físico
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Detectar si toca el suelo usando un círculo invisible
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Corrion"))
        {
            isOnFood = true;
            currentFood = collision.gameObject;
            Debug.Log("Press Down to eat!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Corrion"))
        {
            isOnFood = false;
            currentFood = null;
        }
    }

    void Spin()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void TakeDamage(float amount)
    {
        lifeGuanaco -= amount;
        if (lifeGuanaco <= 0) Debug.Log("Game Over");
        Die();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(salivaPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().targetTag = "Player"; // If it's an enemy shooting
    }

    void Eat()
    {
        lifeGuanaco += 20f;
        currentAmmo += 5;
        Destroy(currentFood);
        isOnFood = false;
    }

    void PerformAttack()
    {
        // 1. Obtener la posición del mouse en el mundo (convertir de píxeles de pantalla a coordenadas del juego)
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Nos aseguramos de estar en el plano 2D

        // 2. Calcular la dirección desde la boca (firePoint) hacia el mouse
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // 3. Calcular el ángulo para que el sprite de la saliva "mire" al objetivo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. Instanciar la saliva con esa rotación
        GameObject bullet = Instantiate(salivaPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        // 5. Configurar el proyectil (Asegúrate de que tu script Projectile use esta dirección)
        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.targetTag = "Enemy"; // El Guanaco ataca enemigos
                                      // Le pasamos la dirección calculada si tu script Projectile lo permite
        }
        currentEnemy.TakeDamage(attackDamage); // Si quieres aplicar daño directo al enemigo al disparar
    }

    IEnumerator PerformEat()
    {
        animator.SetTrigger("eat"); // Activa animación de comer

        // Esperamos un momento para que la animación coincida con la recuperación
        yield return new WaitForSeconds(0.5f);

        lifeGuanaco = Mathf.Min(lifeGuanaco + 20f, 100f); // No cura más de 100
        currentAmmo += 5;

        if (currentFood != null) Destroy(currentFood);
        isOnFood = false;
    }
    private void Die()
    {
        animator.SetTrigger("die"); // Activa la animación de muerte
        this.enabled = false; // Desactiva este script para que no puedas moverte
        rb.velocity = Vector2.zero; // Detiene al Guanaco

        // Aquí puedes llamar al GameManager para mostrar el panel de derrota
        //FindObjectOfType<GameManager>().ShowDefeat();
    }



}
