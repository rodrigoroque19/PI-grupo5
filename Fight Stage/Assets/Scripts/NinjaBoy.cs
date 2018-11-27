using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NinjaBoy : MonoBehaviour
{

    Rigidbody2D body;
    SpriteRenderer sprite;
    Animator anim;

    NinjaGirl ninjaGirl;

    public float speed = 5f;
    public float jumpForce = 600f;
    public float GroundRadius = 0.45f;
    public float RadiusAttack = 0.2f;
    public float TimeNextAttack;

    public Vector2 forca = new Vector2(30f, 15f);

    public Transform GroundCheck;
    public Transform AttackCheck;

    public Collider2D collision2D = new Collider2D();

    public Text textPoints;

    public GameObject RespawnNinjaBoy;

    public LayerMask whatIsGround;
    public LayerMask NinjaGirl;
    public NinjaGirl objNinjaBoy;

    bool grounded = false;
    bool doubleJump = true;

    public int PontosQueda;

    //============================================================================================================================================================================   
    void Start()
    {

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        textPoints.text = PontosQueda.ToString();
        ninjaGirl = GameObject.FindObjectOfType<NinjaGirl>();
    }

    //============================================================================================================================================================================
    void Update()
    {
        //VALIDADOR DO SOLO
        grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);
    }

    private void FixedUpdate()
    {
        //CHAMA O ANDAR
        move();


        //VERIFICAR SE ESTA NO CHAO
        if (Input.GetButtonDown("Jump") && grounded == true)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            doubleJump = true;
        }
        //VERIFICA SE PODE DAR PULO DUPLO
        if (Input.GetButtonDown("Jump") && grounded == false && doubleJump == true)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            doubleJump = false;
        }

    //============================================================================================================================================================================
        //INTERVALO DE ATAQUES
        if (TimeNextAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && body.velocity == new Vector2(0, 0))
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
    //============================================================================================================================================================================

    //CRIAÇÃO DO MOVIMENTO
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
    //============================================================================================================================================================================

    //VALIDAR NUMERO DE VIDAS E RESPAWN
    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        //VIDAS
        if (collision2D.gameObject.CompareTag("Death"))
        {
            PontosQueda--;
            textPoints.text = PontosQueda.ToString();
            transform.position = RespawnNinjaBoy.transform.position;
        }

        //RESPAWN
        if (collision2D.gameObject.CompareTag("Respawn"))
        {
            RespawnNinjaBoy = collision2D.gameObject;
        }
    }
    //============================================================================================================================================================================

    //FLIP DO PERSONAGEM
    void Flip()
    {
        sprite.flipX = !sprite.flipX;
        AttackCheck.localPosition = new Vector2(-AttackCheck.localPosition.x, AttackCheck.localPosition.y);

    }
    //============================================================================================================================================================================

    //ATAQUE DO PERSONAGEM COM AÇÃO DA FORÇA
    void PlayerAttack()
    {
        Collider2D[] NinjaBoyAttack = Physics2D.OverlapCircleAll(AttackCheck.position, RadiusAttack, NinjaGirl);
        for (int i = 0; i < NinjaBoyAttack.Length; i++)
        {
            ninjaGirl.AddForce(forca, ForceMode2D.Impulse);
        }
    }
    //============================================================================================================================================================================

    //FUNÇAO QUE ADICIONA FORÇA AO CORPO DE OUTRA CLASSE
    public void AddForce(Vector2 forca, ForceMode2D impulse)
    {
        int b = 5;
        for (int a = 2; a < 20; a += 5)
        {
            forca = new Vector2((float)(a), (float)(b));
            b += 3;
        }
        this.body.AddForce(forca, impulse);

    }
    //============================================================================================================================================================================

    //TORNAR VISIVEL GROUND E ATAQUE CHECK
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundRadius);
        Gizmos.DrawWireSphere(AttackCheck.position, RadiusAttack);
    }
}
