using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    //[SerializeField] private float attackDamage = 15f;

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
    private float verticalInput;
    private EnemyBase currentEnemy;
    private bool isAttacking = false;
    private bool isEating = false;

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
        verticalInput = Input.GetAxisRaw("Vertical");


        // ENVIAR DATOS AL ANIMATOR
        // Usamos Mathf.Abs para que el valor siempre sea positivo (1 si vas a derecha o izquierda)
        if (animator != null)
        {
            animator.SetFloat("velocidadHorizontal", Mathf.Abs(horizontalInput));
            // Enviamos el valor vertical para que el Blend Tree detecte el -1 (Abajo)
            animator.SetFloat("velocidadVertical", verticalInput);
        }

        // 3. Bloqueo de acciones si está comiendo
        if (isEating) return;

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

    private void PerformAttack()
    {
        // 1. Chequeo de munición primero
        if (currentAmmo <= 0)
        {
            Debug.Log("¡Sin munición!");
            return;
        }

        // 2. Lógica del Mouse (Apuntado)
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 3. Instanciar la saliva
        GameObject bullet = Instantiate(salivaPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        // 4. Configurar Proyectil
        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.targetTag = "Enemy"; // El Guanaco daña enemigos
        }

        // 5. RESTAR MUNICIÓN (Vital para la UI)
        currentAmmo--;
        Debug.Log("Munición restante: " + currentAmmo);

        // 6. Animación (Opcional si tenés el Trigger)

        isAttacking = true;
        if (animator != null)
            //animator.SetTrigger("attack");
            animator.SetBool("isAttacking", isAttacking);
    }


    IEnumerator PerformEat()
    {
        isEating = true;

        yield return new WaitForSeconds(0.5f); // Tiempo de la animación

        // Lógica de recuperación
        lifeGuanaco = Mathf.Min(lifeGuanaco + 20f, 100f);
        currentAmmo += 5;

        if (currentFood != null) Destroy(currentFood);

        isOnFood = false;
        isEating = false; // Importante resetear para volver a moverse
    }
    private void Die()
    {
        animator.SetTrigger("die"); // Activa la animación de muerte
        this.enabled = false; // Desactiva este script para que no puedas moverte
        rb.velocity = Vector2.zero; // Detiene al Guanaco
    }
}
