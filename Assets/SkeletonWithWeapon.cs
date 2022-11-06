using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWithWeapon : EnemyBase
{
    public SkeletonWithWeapon() : base(100, 3.0f, 120, 1){}

    [SerializeField]
    private float projectileAttackCooldown = 3.0f; //время перезарядки

    private float lastTimeProjectileAttacked = 0.0f; //начальное время таймера

    [SerializeField]
    private int projectileBurstCount = 10; // количество снарядов в очереди

    [SerializeField]
    private float projectileSpawnDelay = 0.1f;

    [SerializeField]
    private GameObject projectileObj;

    [SerializeField]
    private float projectileOffset = 5f;

    [SerializeField]
    private float desirableDistance = 5f;

    private Vector3 destinationLocation; // точка в которую будет идти скелет (Предыдущее положение игрока)

    private float modifiedProjectileAttackCooldown = 0.0f;


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

        destinationLocation = transform.position;
        modifiedProjectileAttackCooldown = projectileAttackCooldown + Random.Range(0.0f, 1.0f); //

        flashScript = gameObject.GetComponent<FlashScript>();

        //animator.SetTrigger("Walk");
    }

    // Update is called once per frame
    void Update()
    {
        //lastTimeAttacked += Time.deltaTime;
        lastTimeProjectileAttacked += Time.deltaTime;
        
        Move();
        BurstShootingAttack();
        //Attack();

        Vector3 newScale = transform.localScale;
        if (Vector3.Dot((player.transform.position - transform.position), Vector3.left) < 0.0f) //определение, с какой стороны игрок и отражение спрайта в его сторону
        {
            newScale.x = -1;
        }
        else
        {
            newScale.x = 1;
        }
        transform.localScale = newScale;
    }

    public override void Attack()
    {

    }

    public void BurstShootingAttack() // стрельба очередями
    {
        if (!player.GetComponent<LifeController>().IsDead() && lastTimeProjectileAttacked > modifiedProjectileAttackCooldown)
        {
            //animator.SetTrigger("Walk"); // добавить название тригера
            lastTimeProjectileAttacked = 0.0f;
            modifiedProjectileAttackCooldown = projectileAttackCooldown + Random.Range(-2.0f, 1.0f); // задаем новый кулдаун с рандомным отклонением
            StartCoroutine(SpawnProjectile()); // запускаем спавн снарядов
        }

    }
    IEnumerator SpawnProjectile()
    {
        Vector3 toPlayerDirection = player.transform.position - transform.position; // определение направления к игроку

        for (int i = 0; i < projectileBurstCount; i++)
        {
            toPlayerDirection = player.transform.position - transform.position;
            //direction = Quaternion.AngleAxis(rotationAngle, new Vector3(0.0f, 0.0f, 1.0f)) * direction;
            GameObject projectile;
            projectile = Instantiate(projectileObj);
            Vector3 projectileDirection = (toPlayerDirection).normalized;
            projectile.transform.position = transform.position + projectileDirection * projectileOffset;
            projectile.GetComponent<HostileProjectileBehaviour>().SetDirection(projectileDirection);
            yield return new WaitForSeconds(projectileSpawnDelay); // добавил рандом к кд
        }
    }

    public override void Move()
    {

        float modifiedSpeed = maxSpeed; //скорость передвижения
        float modifiedDesirableDistance = desirableDistance; // дистанция до игрока

        //if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "AttackProjectiles")// поменять название клипа
        //{
        //    modifiedSpeed *= 4;
        //    modifiedDesirableDistance /= 2;
        //}
        //else {}
        
        float step = modifiedSpeed * Time.deltaTime;

        //Vector3 movementDirection = player.transform.position - transform.position;
        Vector3 movementDirection = destinationLocation - transform.position; // направление в сторону точки назначения
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;

        float distanceToDestination = (destinationLocation - transform.position).magnitude; // расстояние до точки назначения

        if (distanceToPlayer > modifiedDesirableDistance * 3 + Random.Range(-5.0f, 5.0f)) // если игрок отошел далеко, то определить новую точку
        {
            destinationLocation = player.transform.position; // определяем новую точку как текущее положение игрока
            distanceToDestination = (destinationLocation - transform.position).magnitude; // обновляем дистанцию до новой точки
        }

        if (distanceToDestination < modifiedDesirableDistance + Random.Range(0.0f, 2.0f)) // останавливается если подходит близко к точке
        //if (distanceToPlayer < modifiedDesirableDistance + Random.Range(0.0f, 2.0f)) // останавливается если подходит близко
        {
            movementDirection = movementDirection * 0.0f;
            //animator.SetTrigger("Idle");
        }
        Vector3 movementTowardsPlayer = movementDirection.normalized * step;

        transform.position = transform.position + movementTowardsPlayer;
        rb.velocity = new Vector2(movementTowardsPlayer.x, movementTowardsPlayer.y);
    }
}
