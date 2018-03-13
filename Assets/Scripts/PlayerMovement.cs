using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : NetworkBehaviour
{
    public Character characterType;

    private float speed = 0f;
    private Vector2 movementDirection;
    private Rigidbody2D rbody;

    public Animator animator;

    private SpriteRenderer _renderer;

    private int maxHealth;

    [SyncVar]
    private bool isDead;

    [SyncVar]
    private int curHealth;

    [SyncVar]
    private bool isFlipped;

    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = characterType.animator;
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
        curHealth = maxHealth = characterType.health;
        isDead = false;
        isFlipped = false;
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().material.color = Color.blue;
        GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
    }

    [Command]
    void CmdFlipRenderer(bool value)
    {
        isFlipped = value;
    }

    [Command]
    public void CmdTakeDamage(int dmg)
    {
        if (!isServer)
            return;

        curHealth -= dmg;
        Debug.Log("I'm hurt on the server!!");
        if (curHealth <= 0)
        {
            curHealth = 0;
            GetComponent<NetworkAnimator>().SetTrigger("Die");
            isDead = true;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        rbody.velocity = movementDirection;
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.flipX = isFlipped;

        if (!isLocalPlayer || isDead)
        {
            return;
        }

        Move();
        animator.SetFloat("Speed", speed);
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x < 0f)
            CmdFlipRenderer(true);
        else if (x > 0f)
            CmdFlipRenderer(false);

        Vector2 dir = new Vector2(x, y);
        speed = 0f;
        if (dir.magnitude > 0f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = characterType.runSpeed;
            }
            else
            {
                speed = characterType.walkSpeed;
            }
        }
        movementDirection = dir.normalized * speed;
    }
}
