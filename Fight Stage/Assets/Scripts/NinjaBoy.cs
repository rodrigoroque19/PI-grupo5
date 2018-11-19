﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaBoy : MonoBehaviour
{
    Rigidbody2D body;
    Animator anim;
    SpriteRenderer sp;

    [Header("Movement Variables")]
    public float maxSpeed = 5f;
    bool grounded = true;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundRadius = 0.2f;
    bool doubleJump = false;
    public float jumpForce = 400f;

    [Header("Attack Variables")]
    public Transform attackCheck;
    public float radiusAttack;
    public LayerMask NinjaGirl;
    float timeNextAttack;

    // Use this for initialization
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public float GetAbsRunVel()
    {
        return Mathf.Abs(body.velocity.x);
    }

    void FixedUpdate()
    {
        //verificar se o personagem está no chao
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        move();

        //Criação do pulo
        if (Input.GetButtonDown("Jump") && grounded == true)
        {
            body.AddForce(new Vector2(0, jumpForce));
            doubleJump = true;
        }
        if (Input.GetButtonDown("Jump") && grounded == false && doubleJump == true)
        {
            body.AddForce(new Vector2(0, jumpForce));
            doubleJump = false;
        }

        //Criação do Attack
        if(Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("Attack");
        }

    }
    //Movimento do personagem
    void move()
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.A) == true)
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.D) == true)
        {
            direction.x = 1;
        }

        Vector2 vlc = body.velocity;
        vlc.x = direction.x * maxSpeed;
        body.velocity = vlc;

        anim.SetFloat("Velocity", GetAbsRunVel());
        if (body.velocity.x > 0)
        {
            sp.flipX = false;
        }
        else if (body.velocity.x < 0)
        {
            sp.flipX = true;
        }
    }

    void OnDrawnGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

}
