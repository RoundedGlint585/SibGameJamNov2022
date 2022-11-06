using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    public float maxSpeed = 50.0f;

    [SerializeField]
    public float maxLifeTime = 100.0f; //for sanity sake

    private float lifeTimer = 0.0f;

    [SerializeField]
    private int damage = 50;

    AudioSource audioSource;
    AudioClip wallHit;
    AudioClip enemyHit;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>(); ;

        wallHit = Resources.Load("Sounds/HitWall") as AudioClip;
        enemyHit = Resources.Load("Sounds/HitEnemy") as AudioClip;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Tilemap")
        {
            audioSource.PlayOneShot(wallHit);
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Enemy")
        {
            maxSpeed = 0.0f;
            collision.gameObject.GetComponent<EnemyBase>().RemoveHealth(damage);
            audioSource.PlayOneShot(enemyHit);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if(lifeTimer > maxLifeTime)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = transform.position + direction * Time.deltaTime * maxSpeed;
    }
}
