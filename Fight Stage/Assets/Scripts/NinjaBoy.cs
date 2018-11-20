using System.Collections;
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
    public Transform GroundCheck;
    public LayerMask WhatIsGround;
    public float GroundRadius = 0.2f;
    bool doubleJump = false;
    public float jumpForce = 300f;

    [Header("Attack Variables")]
    public Transform AttackCheck;
    public float RadiusAttack;
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

    void FixedUpdate()
    {
        //verificar se o personagem está no chao
        grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, WhatIsGround);
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
        if (Input.GetKeyDown(KeyCode.F))
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

        //Vector2 vlc = body.velocity;
        //vlc.x = direction.x * maxSpeed;
        anim.SetFloat("Velocity", Mathf.Abs(body.velocity.x));
        body.velocity = new Vector2(direction.x * maxSpeed, body.velocity.y);

        if (direction.x > 0 && sp.flipX == true || direction.x < 0 && sp.flipX == false)
        {
            Flip();
        }
    }

    void Flip()
    {
        sp.flipX = !sp.flipX;
        AttackCheck.localPosition = new Vector2(-AttackCheck.localPosition.x, AttackCheck.localPosition.y);
    }

    void OnDrawnGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundRadius);
        Gizmos.DrawWireSphere(AttackCheck.position, RadiusAttack);
    }

}
