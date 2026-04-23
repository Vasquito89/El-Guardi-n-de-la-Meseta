using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumaController : EnemyBase
{
    protected override void Attack()
    {
        playerController.TakeDamage(15f);
        Debug.Log("Hare attacked the player!");
        Debug.Log($"Player's remaining life: {playerController.lifeGuanaco}");
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
            playerController.TakeDamage(25f);
        }
    }

}
