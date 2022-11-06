using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0);

        /*        if (Input.GetKey(KeyCode.S))
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
                }*/
        UpdateRotation();
    }

    void UpdateRotation()
    {
        float upMovementDegree = 15.0f;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;
        Vector3 lookingDirection = mousePosition - transform.position;
        float angleToUpVector = Vector3.Angle(lookingDirection, Vector3.up);
        bool isLeftHalfCircle = Vector3.Dot(lookingDirection, Vector3.right) < 0.0f;
        bool isLookingCloseToUp = angleToUpVector < 20.0f;
        bool isLookingDiagonally = angleToUpVector < 60.0f && angleToUpVector >= 20.0f;
        bool isLookingLeftRight = angleToUpVector < 135.0f && angleToUpVector >= 60.0f;
        string halfCircleSide = isLeftHalfCircle ? "Left" : "Right";
        
        bool isMovementButtonPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A);
        string modifier = isMovementButtonPressed ? "Walk" : "Idle";
        animator.SetTrigger(modifier);
        if (isLookingCloseToUp)
        {
            animator.SetTrigger(modifier + "Up");
        }
        else if (isLookingDiagonally)
        {
            animator.SetTrigger(modifier + halfCircleSide + "Up");
        }else if (isLookingLeftRight)
        {
            animator.SetTrigger(modifier  + halfCircleSide);
        }
        else
        {
            animator.SetTrigger(modifier + "Down");
        }
        

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
