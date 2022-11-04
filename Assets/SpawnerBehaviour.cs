using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{

    [SerializeField]
    private float spawnCooldown = 1.0f;

    private float lastTimeSpawned = 0.0f;

    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lastTimeSpawned += Time.deltaTime;
        if(lastTimeSpawned > spawnCooldown)
        {
            GameObject enemyObj = Instantiate(enemy);
            enemyObj.transform.position = transform.position;
            lastTimeSpawned = 0.0f;
            
        }
    }
}
