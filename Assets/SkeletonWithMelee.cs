using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWithMelee : EnemyBase
{

    public SkeletonWithMelee() : base(150, 1.0f, 100, 10)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        levelTilemap = GameObject.FindGameObjectWithTag("Tilemap");
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lastTimeAttacked += Time.deltaTime;
        Move();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Attack();
        }
    }
    public override void Attack()
    {
        if (!player.GetComponent<LifeController>().IsDead() && lastTimeAttacked > attackCooldown)
        {
            lastTimeAttacked = 0.0f;
            player.GetComponent<LifeController>().RemoveHealth(hitStrength);
        }
    }
}
