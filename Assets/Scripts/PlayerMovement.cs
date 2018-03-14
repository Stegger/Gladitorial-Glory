using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : NetworkBehaviour
{
    public Character characterType;
    public RectTransform healthBar;
    public Text txtName;

    private float speed = 0f;
    private Vector2 movementDirection;
    private Rigidbody2D rbody;

    public Animator animator;

    private SpriteRenderer _renderer;

    private int maxHealth;

    [SyncVar(hook = "NameUpdate")]
    private string name;

    [SyncVar]
    private bool isDead;

    [SyncVar(hook = "UpdateHealthBar")]
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

    public void changeName()
    {
        CmdChangeName(txtName.name.Trim());
    }

    [Command]
    public void CmdChangeName(string name)
    {
        this.name = name;
    }

    public override void OnStartLocalPlayer()
    {
        GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
        string name = GameObject.Find("Name Field").GetComponent<Text>().text.Trim();
        CmdChangeName(name);
        Destroy(GameObject.Find("Name Field"));
    }

    public void NameUpdate(string name)
    {
        txtName.text = name;
    }

    public void UpdateHealthBar(int curHealth)
    {
        healthBar.sizeDelta = new Vector2(curHealth, healthBar.sizeDelta.y); 
    }

    [Command]
    void CmdFlipRenderer(bool value)
    {
        if (isFlipped != value)
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            Vector2 offset = box.offset;
            offset.x = offset.x * -1;
            box.offset = offset;
            isFlipped = value;
        }
    }

    [Command]
    public void CmdTakeDamage(int dmg)
    {
        if (!isServer)
            return;

        curHealth -= dmg;
        if (curHealth <= 0)
        {
            curHealth = 0;
            isDead = true;
            GetComponent<NetworkAnimator>().SetTrigger("Die");
        }
    }



    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        CmdSetMovementDirection(movementDirection);
    }

    [Command]
    private void CmdSetMovementDirection(Vector2 dir)
    {
        rbody.velocity = dir;
        animator.SetFloat("Speed", dir.magnitude);
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
