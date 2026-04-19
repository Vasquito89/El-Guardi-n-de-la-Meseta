using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuanacoController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Leer input de flechas o WASD
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Saltar si presiona Espacio y está tocando el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Girar el sprite según la dirección
        if (horizontalInput > 0 && !facingRight) Girar();
        else if (horizontalInput < 0 && facingRight) Girar();
    }

    void FixedUpdate()
    {
        // Aplicar movimiento físico
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Detectar si toca el suelo usando un círculo invisible
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void Girar()
    {
        facingRight = !facingRight;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}
