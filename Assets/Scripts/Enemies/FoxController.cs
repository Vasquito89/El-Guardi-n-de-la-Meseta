using UnityEngine;

public class FoxController : EnemyBase
{
    protected override void Attack()
    {
        //playerController.TakeDamage(10f);
        Debug.Log("Hare attacked the player!");
        Debug.Log($"Player's remaining life: {playerController.lifeGuanaco}");
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        rb.velocity = Vector2.zero;
        soundController.StopWalk();
        soundController.PlayAction(); // The shooting sound
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.TakeDamage(15f);
        }
    }
}
