using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    public float timeToRespawn;

    public float[] dano = new float[2];
    

    public Transform groundCheck;
    public Transform attackCheck;

    public Text textPontos;

    public GameObject RespawnNinjaGirl;

    public LayerMask whatIsGround;
    public LayerMask NinjaBoy;
    public NinjaBoy objNinjaBoy;

    bool doubleJump = false;
    bool grounded = false;

    public int pontosQueda;

    // Use this for initialization
    void Start()
    {

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        textPontos.text = pontosQueda.ToString();

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
            var a = GameObject.Find("NinjaBoy");
            

            if (Input.GetKeyDown(KeyCode.RightControl) && body.velocity == new Vector2(0, 0))
            {
                anim.SetTrigger("Attack");
                timeNextAttack = 0.2f;
                PlayerAttack();
                var arma = GameObject.FindWithTag("NinjaBoy").GetComponent<NinjaBoy>();
                
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

 
    //Verificador de colisoes
    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        //Validador de vidas
        if (collision2D.gameObject.CompareTag("Death"))
        {
            pontosQueda--;
            textPontos.text = pontosQueda.ToString();
            transform.position = RespawnNinjaGirl.transform.position;

        }

        //Validador de Respawn
        if (collision2D.gameObject.CompareTag("Respawn"))
        {
            RespawnNinjaGirl = collision2D.gameObject;
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
            Debug.Log(NinjaBoyAttack[i].name);
            dano[0] = 300f;
            dano[1] = 30f;
            
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
