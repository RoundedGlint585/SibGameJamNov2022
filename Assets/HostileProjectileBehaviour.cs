using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class HostileProjectileBehaviour : MonoBehaviour
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
        audioSource = GetComponent<AudioSource>();

        wallHit = Resources.Load("Sounds/HitWall") as AudioClip;
        enemyHit = Resources.Load("Sounds/HitEnemy") as AudioClip;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tilemap")
        {
            Destroy(gameObject);
            audioSource.PlayOneShot(wallHit);
        }
        else if (collision.gameObject.tag == "Player")
        {
            maxSpeed = 0.0f;
            Destroy(gameObject);
            collision.gameObject.GetComponent<LifeController>().RemoveHealth(damage);
            audioSource.PlayOneShot(enemyHit);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>().isGameFreezed)
        {
            return;
        }
        lifeTimer += Time.deltaTime;
        if (lifeTimer > maxLifeTime)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = transform.position + direction * Time.deltaTime * maxSpeed;
    }
}
