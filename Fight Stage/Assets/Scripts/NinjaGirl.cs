using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaGirl : MonoBehaviour
{
    Rigidbody2D body;
    Animator anim;
    //Indicar o lado que o personagem esta olhando
    bool facingRight = true;

    //Adicionar velocidade
    public float maxSpeed = 5f;

    bool grounded = true;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundRadius = 0.2f;
    bool jump = false;

    // Use this for initialization
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
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

        //Movimento do personagem
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Velocity", Mathf.Abs(move));

        body.velocity = new Vector2(move * maxSpeed, body.velocity.y);

        if (move > 0 && facingRight == true)
        {
            Flip();
        }

        if (move < 0 && facingRight == false)
        {
            Flip();
        }


    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
