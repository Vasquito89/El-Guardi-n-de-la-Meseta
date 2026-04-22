using UnityEngine;

public class CazadorController : MonoBehaviour
{
    [Header("Configuración de IA")]
    [SerializeField] private Transform jugador;          // Arrastra aquí al Guanaco
    [SerializeField] private float velocidadBusqueda = 3f;
    [SerializeField] private float distanciaDeteccion = 10f;
    [SerializeField] private float distanciaAtaque = 2f;

    [Header("Componentes")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (jugador == null) return;

        // Calcular distancia actual
        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= distanciaAtaque)
        {
            Attack();
        }
        else if (distancia <= distanciaDeteccion)
        {
            Chase();
        }
        else
        {
            StayStill();
        }
    }

    void StayStill()
    {
        // Detener animación de caminar
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        // Detenemos la velocidad física al estar quietos
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Chase()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);

        // Calculamos dirección horizontal
        float direccionX = (jugador.position.x - transform.position.x);
        direccionX = Mathf.Sign(direccionX); // Nos da 1 o -1

        // Aplicamos velocidad al Rigidbody (mejor que mover el transform)
        rb.velocity = new Vector2(direccionX * velocidadBusqueda, rb.velocity.y);

        // Girar el sprite
        spriteRenderer.flipX = (jugador.position.x > transform.position.x);
    }

    void Attack()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        // Frenar al atacar
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}