using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumaController : EnemyBase
{
    private GuanacoController playerController;

    protected override void Attack()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        rb.velocity = Vector2.zero;
        soundController.StopWalk();
        soundController.PlayAction(); // The shooting sound
        playerController.TakeDamage(25f);
        Debug.Log("Hare attacked the player!");
        Debug.Log($"Player's remaining life: {playerController.lifeGuanaco}");
    }

}
