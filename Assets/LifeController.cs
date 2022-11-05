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

    private UIBehaviour UIRef; // ссылка на UI для доступа к хп игрока

    void Start()
    {
        health = healthMax;
        UIRef = GameObject.Find("UI_Main").GetComponentInChildren<UIBehaviour>();
        if (UIRef == null)
            Debug.Log("ui is null");
        else
            Debug.Log("ui is NOT null");
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void AddHealth(int toAdd)
    {
        health += toAdd;
        health = health > healthMax ? healthMax : health;
        UIRef.UpdateHPCount(health); // обновление информации о хп в интерфейе
    }

    public void RemoveHealth(int toRemove)
    {
        health -= toRemove;
        health = health < 0 ? 0 : health;
        UIRef.UpdateHPCount(health); // обновление информации о хп в интерфейе
    }

    public int GetHealth()
    {
        return health;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
