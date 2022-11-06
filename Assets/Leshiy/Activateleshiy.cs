using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activateleshiy : MonoBehaviour
{
    [SerializeField] private GameObject Boss;
    void Start()
    {
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Boss.active = true;
    }
}
