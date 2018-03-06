using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float WalkSpeed = 1f;
    public float RunSpeed = 2f;
    public Ability ability;

    private float speed = 0f;
    private Vector2 movementDirection;
    private Rigidbody2D rbody;
    private Animator animator;
    private SpriteRenderer _renderer;

    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        speed = WalkSpeed;
        ability.Initialize(transform.gameObject);
    }



    private void FixedUpdate()
    {
        rbody.velocity = movementDirection;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Die");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
            Debug.Log("Triggering ability " + ability.name);
            ability.TriggerAbility();
            movementDirection = Vector2.zero;
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x < 0f)
            _renderer.flipX = true;
        else if (x > 0f)
            _renderer.flipX = false;

        Vector2 dir = new Vector2(x, y);
        speed = 0f;
        if (dir.magnitude > 0f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = RunSpeed;
            }
            else
            {
                speed = WalkSpeed;
            }
        }
        movementDirection = dir.normalized * speed;
        animator.SetFloat("Speed", speed);
    }
}
