using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HareController : EnemyBase
{
    private GuanacoController playerController;

    protected override void Attack()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        rb.velocity = Vector2.zero;
        soundController.StopWalk();
        soundController.PlayAction(); // The shooting sound
        playerController.TakeDamage(15f); // Damage to the player
        Debug.Log("Hare attacked the player!");
        Debug.Log($"Player's remaining life: {playerController.lifeGuanaco}");
    }
}
