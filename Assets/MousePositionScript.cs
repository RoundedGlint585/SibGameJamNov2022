using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionScript : MonoBehaviour
{

    [SerializeField]
    private float maxRadius = 100.0f;

    [SerializeField]
    private float minRadius = 30.0f;

    public GameObject mainCharacter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;

        Vector3 characterPosition = mainCharacter.transform.position;
        Vector3 direction = mousePosition - characterPosition;
        float length = direction.magnitude;
        if(length > maxRadius)
        {
            length = maxRadius;
        }
        if(length < minRadius)
        {
            length = minRadius;
        }
        transform.position = characterPosition + direction.normalized * length;
    }
}
