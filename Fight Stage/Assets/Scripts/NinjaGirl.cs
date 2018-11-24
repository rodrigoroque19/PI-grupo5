using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaGirl : MonoBehaviour
{

    Rigidbody2D body;
    SpriteRenderer sprite;
    Animator anim;

    public float speed = 5f;
    public float jumpForce = 600f;
    public float groundRadius = 0.4f;
    public float radiusAttack = 0.2f;
    public float timeNextAttack;

    public Transform groundCheck;
    public Transform attackCheck;

    public LayerMask whatIsGround;
    public LayerMask NinjaBoy;

    bool doubleJump = false;
    bool grounded = false;

    // Use this for initialization
    void Start()
    {

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //verificação do solo
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

    }

    private void FixedUpdate()
    {
        move();
        
        //verificação do pulo
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded == true)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            doubleJump = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded == false && doubleJump == true)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            doubleJump = false;
        }

        //Criação do ataque e do intervalo de golpes
        if (timeNextAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.RightControl) && body.velocity == new Vector2(0, 0))
            {
                anim.SetTrigger("Attack");
                timeNextAttack = 0.2f;
                PlayerAttack();
            }            
        }
        else
        {
            timeNextAttack -= Time.deltaTime;
        }

    }

    //Função de andar
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

        anim.SetFloat("Velocity", Mathf.Abs(body.velocity.x));
        body.velocity = new Vector2(direction.x * speed, body.velocity.y);

        if (direction.x > 0 && sprite.flipX == false || direction.x < 0 && sprite.flipX == true)
        {
            Flip();
        }
    }

    //Função que altera a escala para que o personagem vire pra direção que esta andando
    void Flip()
    {
        sprite.flipX = !sprite.flipX;
        attackCheck.localPosition = new Vector2(-attackCheck.localPosition.x, attackCheck.localPosition.y);
    }

    void PlayerAttack()
    {
        Collider2D[] NinjaBoyAttack = Physics2D.OverlapCircleAll(attackCheck.position, radiusAttack, NinjaBoy);
        for (int i = 0; i < NinjaBoyAttack.Length; i++)
        {
            NinjaBoyAttack[i].SendMessage("Enemy Hit");
            Debug.Log(NinjaBoyAttack[i].name);
        }
    }

    //Função para que fique visivel o contorno do GroundCheck e AttackCheck
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        Gizmos.DrawWireSphere(attackCheck.position, radiusAttack);
    }
}
