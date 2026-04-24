using System.Collections;
using UnityEngine;

public class HunterController : EnemyBase
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    protected override void Attack()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        rb.velocity = Vector2.zero;
        soundController.StopWalk();
        soundController.PlayAction(); // The shooting sound
        Shoot();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().targetTag = "Player"; // If it's an enemy shooting
        Debug.Log("Hare attacked the player!");
        Debug.Log($"Player's remaining life: {playerController.lifeGuanaco}");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.TakeDamage(25f);
        }
    }
}