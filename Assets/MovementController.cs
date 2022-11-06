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
    bool isDeathAnimationPlayed = false;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isDead = GetComponent<LifeController>().IsDead();
        if (!isDead)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, vertical, 0);
        }
        UpdateRotation();
    }

    void UpdateRotation()
    {
        bool isDead = GetComponent<LifeController>().IsDead();
        if (isDead)
        {
            if (isDeathAnimationPlayed)
            {
                return;
            }
            else
            {
                animator.SetBool("Death", true);
                animator.SetTrigger("TriggerDeath");
                transform.GetChild(1).gameObject.SetActive(false);
                isDeathAnimationPlayed = true;
            }
        }
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
        bool isDead = GetComponent<LifeController>().IsDead();
        if (!isDead)
        {
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        }
    }
}
