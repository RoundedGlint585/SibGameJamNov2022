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

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Destroy(gameObject);
        }
    }

/*    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }*/
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
