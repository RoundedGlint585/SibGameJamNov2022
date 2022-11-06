using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleEnemy : EnemyBase
{
    [SerializeField]
    private float projectileAttackCooldown = 5.0f;

    private float lastTimeProjectileAttacked = 0.0f;

    [SerializeField]
    private int projectileCircularCount = 10;

    [SerializeField]
    private int projectileConeCount = 5;
    [SerializeField]
    private float coneSpreadSize = 10.0f;

    [SerializeField]
    private float projectileSpawnDelay = 0.1f;

    [SerializeField]
    private GameObject projectileObj;

    [SerializeField]
    private float projectileOffset = 5f;

    [SerializeField]
    private float desirableDistance = 5f;
    public TentacleEnemy() : base(150, 3.0f, 100, 10)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        levelTilemap = GameObject.FindGameObjectWithTag("Tilemap");
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();

        deathSound = Resources.Load("Sounds/EnemyDeath") as AudioClip;
        projectileObj = Resources.Load("Prefabs/HostileProjectileBehaviour") as GameObject; 

        animator = GetComponent<Animator>();
        flashScript = gameObject.GetComponent<FlashScript>();
    }

    // Update is called once per frame
    void Update()
    {
        lastTimeAttacked += Time.deltaTime;
        lastTimeProjectileAttacked += Time.deltaTime;
        Move();
        CirclularAttack();
        Attack();

        Vector3 newScale = transform.localScale;
        if (Vector3.Dot((player.transform.position - transform.position), Vector3.left) < 0.0f)
        {
            newScale.x = -1;
        }
        else
        {
            newScale.x = 1;
        }
        transform.localScale = newScale;
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
            animator.SetTrigger("AttackMelee");
            lastTimeAttacked = 0.0f;
            float changeStep = 2 * coneSpreadSize / projectileConeCount;
            Vector3 toPlayerDirection = player.transform.position - transform.position;
            for (int i = 0; i < projectileConeCount; i++)
            {
                GameObject projectile;
                projectile = Instantiate(projectileObj);
                Quaternion quaternion = Quaternion.Euler(0, 0, -coneSpreadSize + i * changeStep);
                Vector3 projectileDirection = (quaternion * toPlayerDirection).normalized;
                projectile.GetComponent<HostileProjectileBehaviour>().SetDirection(projectileDirection);
                projectile.transform.position = transform.position + projectileDirection * projectileOffset;
            }
        }

    }

    public void CirclularAttack()
    {
        if(!player.GetComponent<LifeController>().IsDead()  && lastTimeProjectileAttacked > projectileAttackCooldown)
        {
            animator.SetTrigger("AttackProjectiles");
            lastTimeProjectileAttacked = 0.0f;
            StartCoroutine(SpawnProjectile());
        }
       
    }
    IEnumerator SpawnProjectile()
    {
        Vector3 direction = new Vector3(-1.0f, 0.0f, 0.0f);
        float rotationAngle = 360.0f / projectileCircularCount;
        for (int i = 0; i < projectileCircularCount; i++)
        {
            direction = Quaternion.AngleAxis(rotationAngle, new Vector3(0.0f, 0.0f, 1.0f)) * direction;
            GameObject projectile;
            projectile = Instantiate(projectileObj);
            projectile.transform.position = transform.position + direction * projectileOffset;
            projectile.GetComponent<HostileProjectileBehaviour>().SetDirection(direction);
            yield return new WaitForSeconds(projectileSpawnDelay);
        }
    }

    public override void Move()
    {

        float modifiedSpeed = maxSpeed;
        float modifiedDesirableDistance = desirableDistance;
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "AttackProjectiles")
        {
            modifiedSpeed *= 4;
            modifiedDesirableDistance /= 2;
        }
        else
        {

        }
        float step = modifiedSpeed * Time.deltaTime;
 
        Vector3 movementDirection = player.transform.position - transform.position;
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;

        if (distanceToPlayer < modifiedDesirableDistance + Random.Range(0.0f, 1.0f))
        {
            movementDirection = -movementDirection;
        }
        Vector3 movementTowardsPlayer = movementDirection.normalized * step;

        transform.position = transform.position + movementTowardsPlayer;
        rb.velocity = new Vector2(movementTowardsPlayer.x, movementTowardsPlayer.y);
        

    }

}
