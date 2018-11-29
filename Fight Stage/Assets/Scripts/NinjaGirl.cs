using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NinjaGirl : MonoBehaviour
{

    public Rigidbody2D body;
    public SpriteRenderer sprite;
    Animator anim;

    NinjaBoy ninjaBoy;

    public float speed = 5f;
    public float jumpForce = 400f;
    public float groundRadius = 0.4f;
    public float radiusAttack = 0.2f;
    public float timeNextAttack;
    //public Vector2 forca = new Vector2(50f, 15f);    
    Vector2 forca;
    public Collider2D collision2D = new Collider2D();

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
    //==============================================================================================================================================================================================================================   

    void Start()
    {

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        textPontos.text = pontosQueda.ToString();
        ninjaBoy = GameObject.FindObjectOfType<NinjaBoy>();

    }

    //==============================================================================================================================================================================================================================

    void Update()
    {
        //VALIDADOR DO SOLO
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

    }
    //==============================================================================================================================================================================================================================

    private void FixedUpdate()
    {
        //CHAMA O ANDAR
        move();

        //VERIFICAR SE ESTA NO CHAO
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded == true)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            doubleJump = true;
        }
        //VERIFICA SE PODE DAR PULO DUPLO
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded == false && doubleJump == true)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            doubleJump = false;
        }

        //INTERVALO DE ATAQUES
        if (timeNextAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
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
    //==============================================================================================================================================================================================================================

    //CRIAÇÃO DO MOVIMENTO
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
    //==============================================================================================================================================================================================================================

    //VALIDAR NUMERO DE VIDAS E RESPAWN
    private void OnTriggerEnter2D(Collider2D collision2D)
    {

        //VIDAS
        if (collision2D.gameObject.CompareTag("Death"))
        {
            pontosQueda--;
            textPontos.text = pontosQueda.ToString();
            transform.position = RespawnNinjaGirl.transform.position;

        }

        //RESPAWN
        if (collision2D.gameObject.CompareTag("Respawn"))
        {
            RespawnNinjaGirl = collision2D.gameObject;
        }

    }
    //==============================================================================================================================================================================================================================

    //FLIP DO PERSONAGEM
    void Flip()
    {
        sprite.flipX = !sprite.flipX;
        attackCheck.localPosition = new Vector2(-attackCheck.localPosition.x, attackCheck.localPosition.y);
    }
    //==============================================================================================================================================================================================================================

    //ATAQUE DO PERSONAGEM COM AÇÃO DA FORÇA
    void PlayerAttack()
    {

        Collider2D[] NinjaGirlAttack = Physics2D.OverlapCircleAll(attackCheck.position, radiusAttack, NinjaBoy);

        for (int i = 0; i < NinjaGirlAttack.Length; i++)
        {
            if (this.sprite.flipX)
            {
                float x = ninjaBoy.body.position.x + 2;
                float y = ninjaBoy.body.position.y + 1;

                ninjaBoy.body.MovePosition(new Vector2(x, y));
            }
            else
            {

                float x = ninjaBoy.body.position.x - 2;
                float y = ninjaBoy.body.position.y + 1;
                ninjaBoy.body.MovePosition(new Vector2(x, y));
            }
        }
    }
    //==============================================================================================================================================================================================================================

    //FUNÇAO QUE ADICIONA FORÇA AO CORPO DE OUTRA CLASSE
    public void AddForce(Vector2 forca, ForceMode2D impulse)
    {
        int x = 30;
        int y = 20;
        for (int a = 2; a < 20; a += 5)
        {
            forca = new Vector2((float)(x), (float)(y));
           
        }
        this.body.AddForce(forca, impulse);

    }
    //==============================================================================================================================================================================================================================

    //TORNAR VISIVEL GROUND E ATAQUE CHECK
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        Gizmos.DrawWireSphere(attackCheck.position, radiusAttack);
    }
}
