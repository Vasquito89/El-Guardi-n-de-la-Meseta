using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HareController : EnemyBase
{
    protected override void Attack()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        rb.velocity = Vector2.zero;
        soundController.StopWalk();
        soundController.PlayAction(); // The shooting sound
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.TakeDamage(5f); // Damage to the player on collision
            Debug.Log("Hare attacked the player!");
            Debug.Log($"Player's remaining life: {playerController.lifeGuanaco}");
        }
    }
}
