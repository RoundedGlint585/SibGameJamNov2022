using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    [SerializeField]
    float speed = 1.0f;

    private float horizontal;
    private float vertical;

    Rigidbody2D rb;
    // Start is called before the first frame update

    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0);
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetTrigger("WalkDown");
        }
        else if (Input.GetKey(KeyCode.W) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            animator.SetTrigger("WalkUp");
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            animator.SetTrigger("WalkLeftUp");
        }
        else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            animator.SetTrigger("WalkRightUp");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetTrigger("WalkLeft");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetTrigger("WalkRight");
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
