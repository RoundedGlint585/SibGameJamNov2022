using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !transform.parent.gameObject.GetComponent<RoomBehaviour>().isInRoom)
        {
            transform.parent.gameObject.GetComponent<RoomBehaviour>().ActivateRoom();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
