using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{

    [SerializeField]
    private int healthMax = 200;

    [SerializeField]
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = healthMax;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void AddHealth(int toAdd)
    {
        health += toAdd;
        health = health > healthMax ? healthMax : health;
    }

    public void RemoveHealth(int toRemove)
    {
        health -= toRemove;
        health = health < 0 ? 0 : health;   

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
