using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public abstract class EnemyBase : MonoBehaviour
{
    public int health;
    public float attackCooldown;
    public float lastTimeAttacked;

    public float maxSpeed;
    public int hitStrength;
    
    protected GameObject levelTilemap;
    protected GameObject player;
    protected Rigidbody2D rb;
    public Sprite sprite;

    protected AudioSource audioSource;
    protected AudioClip deathSound;

    protected Animator animator;
    public FlashScript flashScript;
    protected EnemyBase(int health, float attackCooldown, float maxSpeed, int hitStrength)
    {
        this.health = health;
        this.attackCooldown = attackCooldown;
        this.maxSpeed = maxSpeed;
        this.hitStrength = hitStrength;
    }

    public void RemoveHealth(int toRemove)
    {
        health -= toRemove;
        flashScript.Flash();
        if(health <= 0)
        {
            audioSource.PlayOneShot(deathSound);
            Destroy(this.gameObject);
        }
    }
    public abstract void Attack();
    public virtual void Move()
    {
        float step = maxSpeed * Time.deltaTime;
        Vector3 movementTowardsPlayer = (player.transform.position - transform.position).normalized * step;
        transform.position = transform.position + movementTowardsPlayer;
        rb.velocity = new Vector2(movementTowardsPlayer.x, movementTowardsPlayer.y);
    }
}
