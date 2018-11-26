using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NinjaBoy : MonoBehaviour
{

    Rigidbody2D body;
    SpriteRenderer sprite;
    Animator anim;

    public float speed = 5f;
    public float jumpForce = 600f;
    public float GroundRadius = 0.45f;
    public float RadiusAttack = 0.2f;
    public float TimeNextAttack;
    
    public Transform GroundCheck;
    public Transform AttackCheck;

    public Text textPoints;

    public GameObject RespawnNinjaBoy;
    
    public LayerMask whatIsGround;
    public LayerMask NinjaGirl;

    bool grounded = false;
    bool doubleJump = true;

    public int PontosQueda;

    // Use this for initialization
    void Start()
    {

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        textPoints.text = PontosQueda.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        //verificação do solo
        grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);
    }

    private void FixedUpdate()
    {
        move();


        //verificação do pulo
        if (Input.GetButtonDown("Jump") && grounded == true)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            doubleJump = true;
        }
        if (Input.GetButtonDown("Jump") && grounded == false && doubleJump == true)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            doubleJump = false;
        }

        //Criação do ataque e do intervalo de golpes
        if (TimeNextAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.T) && body.velocity == new Vector2(0, 0))
            {
                anim.SetTrigger("Attack");
                TimeNextAttack = 0.2f;
                PlayerAttack();
            }
        }
        else
        {
            TimeNextAttack -= Time.deltaTime;
        }
    }

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

        anim.SetFloat("Velocity", Mathf.Abs(body.velocity.x));
        body.velocity = new Vector2(direction.x * speed, body.velocity.y);

        if (direction.x > 0 && sprite.flipX == true || direction.x < 0 && sprite.flipX == false)
        {
            Flip();
        }
    }

    //Verificador de colisoes
    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        //Validador de vidas
        if (collision2D.gameObject.CompareTag("Death"))
        {
            PontosQueda--;
            textPoints.text = PontosQueda.ToString();
            transform.position = RespawnNinjaBoy.transform.position;
        }

        //Validador de Respawn
        if (collision2D.gameObject.CompareTag("Respawn"))
        {
            RespawnNinjaBoy = collision2D.gameObject;
        }
    }

    //Função pra virar o player pra onde esta indo
    void Flip()
    {
        sprite.flipX = !sprite.flipX;
        AttackCheck.localPosition = new Vector2(-AttackCheck.localPosition.x, AttackCheck.localPosition.y);
    }

    //Função do ataque
    void PlayerAttack()
    {
        Collider2D[] NinjaGirlAttack = Physics2D.OverlapCircleAll(AttackCheck.position, RadiusAttack, NinjaGirl);
        for (int i = 0; i < NinjaGirlAttack.Length; i++)
        {
            //NinjaGirlAttack[i].SendMessage("NinjaGirl Hit");
            Debug.Log(NinjaGirlAttack[i].name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundRadius);
        Gizmos.DrawWireSphere(AttackCheck.position, RadiusAttack);
    }
}
