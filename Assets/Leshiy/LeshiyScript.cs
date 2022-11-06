using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeshiyScript : MonoBehaviour
{
    public int health;

    private Animator anim;
    [SerializeField] private GameObject BoxObject;
    private Vector2 _positionSpawn;
    private bool _isAttack = true;
    private GameObject player;
    [SerializeField] private GameObject projectileObj;

    [SerializeField] private FlashScript flashScript;

    [SerializeField]
    private float projectileAttackCooldown = 3.0f; //время перезарядки

    private float lastTimeProjectileAttacked = 0.0f; //начальное время таймера

    [SerializeField]
    private int projectileBurstCount = 10; // количество снарядов в очереди

    [SerializeField]
    private float projectileSpawnDelay = 0.1f;

    private float modifiedProjectileAttackCooldown = 0.0f;

    [SerializeField]
    private float projectileOffset = 5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject SpawnPos = GameObject.FindGameObjectWithTag("SpawnBox");
         _positionSpawn = SpawnPos.transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        if (_isAttack)
        {
            AttackTwo();
        } 
    }

    private void AttackOne()
    {
        anim.SetBool("AttackOne", true);
        Instantiate(BoxObject, _positionSpawn, Quaternion.identity);
    }

    private void AttackTwo()
    {
        anim.SetBool("AttackOne", true);
        BurstShootingAttack();
    }

    public void BurstShootingAttack() // стрельба очередями
    {
        if (!player.GetComponent<LifeController>().IsDead() && lastTimeProjectileAttacked > modifiedProjectileAttackCooldown)
        {
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
            _isAttack = true;

        }
    }

    public void RemoveHealth(int toRemove)
    {
        health -= toRemove;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
