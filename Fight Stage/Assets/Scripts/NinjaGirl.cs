using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaGirl : MonoBehaviour
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
    public float jumpForce = 300f;

    [Header("Attack Variables")]
    public Transform attackCheck;
    public float radiusAttack;
    public LayerMask NinjaBoy;
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
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        move();

        //Criação de pulo
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded == true)
        {
            body.AddForce(new Vector2(0, jumpForce));
            doubleJump = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded == false && doubleJump == true)
        {
            body.AddForce(new Vector2(0, jumpForce));
            doubleJump = false;
        }

        //Criação do Attack
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            anim.SetTrigger("Attack");
        }

    }

    //Movimento do personagem
    void move()
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            direction.x = 1;

        }

        //Vector2 vlc = body.velocity;
        //vlc.x = direction.x * maxSpeed;
        anim.SetFloat("Velocity", Mathf.Abs(body.velocity.x));
        body.velocity = new Vector2(direction.x * maxSpeed, body.velocity.y);

        if (direction.x > 0 && sp.flipX == false || direction.x < 0 && sp.flipX == true)
        {
            Flip();
        }
    }

    void Flip()
    {
        sp.flipX = !sp.flipX;
        attackCheck.localPosition = new Vector2(-attackCheck.localPosition.x, attackCheck.localPosition.y);
    }

    void OnDrawnGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        Gizmos.DrawWireSphere(attackCheck.position, radiusAttack);
    }
}
