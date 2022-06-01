using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    //velocita personaggio
   [SerializeField] private float speed; 
   
   //forza applicata nel salto
   [SerializeField] private float jumpForce;
   
   //layer contenente tutti gli oggetti che rappresentano il ground
   [SerializeField] private LayerMask groundLayer;
   
   //vita del player
   [SerializeField]private int maxHealth = 100;
   private int currentHealth;
   
   //layer contenente tutti gli oggetti che rappresentano il wall
  // [SerializeField] private LayerMask wallLayer;
   
   //riferimento alla barra della vita
   [SerializeField] HealthBar healthBar;
   //flag per controllare se il player si rivolge verso destra
   private bool facingRight = true;
   
   //gestisce le  animazioni
   private Animator anim;
   //gestisce gli input provenienti dall'asse x
   private float xInput;
   
   //gestisce gli input provenienti dall'asse y
   private float yInput;
   
   //consente di attuare la gravità nell'oggetto
   private Rigidbody2D body;
   
   //consente di attuare le collisioni
   private BoxCollider2D boxCollider;
   
   
   //blocco variabili per la gestione del salto anticipato
   private float hangTime = 2f;
   private float hangTimeCounter;
   private float jumpBufferTime = 0.2f;
   private float jumpBufferCounter;    

   private bool grounded;
   private bool jump;
   private bool run;
   private bool attack ;
   
   
   private Vector2 rollDir;
   private float rollSpeed;

   public Transform attackPoint;
   private float attackRange = 0.5f;
   
   private enum State
   {
       normal,rolling
   }




   private State state;
   // Start is called before the first frame update
   private void Start()
   { 
        currentHealth = maxHealth;
        healthBar.SetHealthBarMaxValue(maxHealth);
        print(healthBar);

        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        state = State.normal;
   }

   // metodo che gestisce gli input e le animazioni
    private void Update()
    {
        switch (state)
        {
            case State.normal:
                HandleInput();
                HandleAnimations();
                break;
            
            case State.rolling:
                
                float rollSpeedMultiplier = 1.5f; 
                rollSpeed -= rollSpeed * rollSpeedMultiplier * Time.deltaTime;
                float rollSpeedMinimum = 0.1f;
                
               
                    if (rollSpeed < 1f)
                {
                    
                    state =  State.normal;

                }
                
                
                break;
        }
        
        
          
                






    }
    
    //vanno messe le parti che sfruttano la fisica di unity
    private void FixedUpdate()
    {
        switch (state)
        { 
            case State.normal:
                
                xInput = Input.GetAxis("Horizontal");
                yInput = Input.GetAxis("Vertical");
        
                body.velocity = new Vector2(xInput * speed, body.velocity.y );
       

                if (xInput > 0 && facingRight != true)
                {
                    Flip();
                }
        
                if (xInput < 0 && facingRight == true)
                {
                    Flip();
                }
        
        
                if (IsGrounded())
                {
                    grounded = true;
                    hangTimeCounter = hangTime;
                }
                else
                {
                    grounded = false;
                    hangTimeCounter -= Time.deltaTime;
                }
        
        
                if (jump)
                {
                    Jump();
                    jump = false;
                }

                break;
            
            case State.rolling:
                print("roll State");
                
                print(" rollSpeed"+rollSpeed+"\n");
                print(" rollDir"+rollDir+"\n");
                
                body.velocity = rollSpeed*rollDir; 
               

                break;
                

        }
        
        
        
       
    }
    
    
    

    private void Jump()
    {
        //ForceMode2D.Impulse is useful if Jump() is called using GetKeyDown
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        
        anim.SetBool("try",true);
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;

    }

    private void Flip()
    {

        Vector2 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;

    }

    private Boolean IsGrounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down,0.1f,groundLayer);
        return raycast.collider !=null;
    }
    
    private void HandleAnimations()
    {   
        anim.SetBool("run",xInput != 0);
        if (grounded)
        {
            anim.SetBool("grounded", true);
            
            
        }else {
            anim.SetBool("grounded", false);

            //Set the animator velocity equal to 1 * the vertical direction in which the player is moving 
            if (body.velocity.y < 0)
            {
                anim.SetBool("try",false);
                anim.SetTrigger("fall");
            }
           
        }
        if (currentHealth<=0)
        {
            anim.SetTrigger("death");
            currentHealth = maxHealth;
        }
    }

    private void HandleInput()
    {

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;

        }
        else
        {
            jumpBufferCounter -= jumpBufferTime;
        }
        
        
        if (jumpBufferCounter>0 && hangTimeCounter>0 )
        {
            jump = true;
        }


        if (Input.GetKey(KeyCode.C) && IsGrounded())
        {
            attack = true;
            Attack();

        }
        
        

        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftArrow) && IsGrounded())
        {
            anim.SetTrigger("roll");
            rollDir = Vector2.left;
            rollSpeed = speed;
            state = State.rolling;


        }
        
        
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.RightArrow) && IsGrounded())
        {
            anim.SetTrigger("roll");
            
            rollDir = Vector2.right;
            state = State.rolling;
            rollSpeed =  speed;
        }
        

        
    }


    private void Attack()
    {
        anim.SetTrigger("attack_1");
        
    }
    
    
}
