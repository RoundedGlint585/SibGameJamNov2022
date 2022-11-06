using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Vector2 _characterTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Character = GameObject.FindGameObjectWithTag("Player");
        _characterTransform = Character.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Boom()
    {

    }
}
