using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject roomTrigger;

    public bool isInRoom = false;

    public bool isEnemiesDead = false;
    // Start is called before the first frame update
    void Start()
    {
   
    }
    
    public void ActivateRoom()
    {
        isInRoom = true;
        GameObject doors = transform.GetChild(1).gameObject;
        for(int i = 0; i < doors.transform.childCount; i++)
        {
            doors.transform.GetChild(i).gameObject.SetActive(true);
        }
        GameObject enemies = transform.GetChild(2).gameObject;
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            enemies.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void DeactivateRoom()
    {
        GameObject doors = transform.GetChild(1).gameObject;
        for (int i = 0; i < doors.transform.childCount; i++)
        {
            doors.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        IsEnemiesDeadUpdate();
        if (isEnemiesDead)
        {
            DeactivateRoom();
        }
    }

    void IsEnemiesDeadUpdate()
    {
        GameObject enemiesGameObject = transform.GetChild(2).gameObject;
        isEnemiesDead = enemiesGameObject.transform.childCount == 0;

    }
}
