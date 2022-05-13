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
   
   private bool grounded;
   private bool jump;
   private int maxHealth = 100;
   private int currentHealth;


   // Start is called before the first frame update
   private void Start()
   { 
        currentHealth = maxHealth;
        healthBar.SetHealthBarMaxValue(maxHealth);
        print(healthBar);

        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
   }

   // metodo che gestisce gli input e le animazioni
    private void Update()
    {
       
        
        anim.SetBool("run",xInput != 0);

        HandleInput();
        HandleAnimations();
        
    }
    
    //vanno messe le parti che sfruttano la fisica di unity
    private void FixedUpdate()
    {
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
        
        
        if (jump && IsGrounded() )
        {
            Jump();
            jump = false;
           
        }
        
        
       
    }
    
    
    

    private void Jump()
    {
        //ForceMode2D.Impulse is useful if Jump() is called using GetKeyDown
        body.AddForce(Vector2.up * body.mass * jumpForce, ForceMode2D.Impulse);
        
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
        if (!IsGrounded() )
        {
            anim.SetBool("grounded", false);

            //Set the animator velocity equal to 1 * the vertical direction in which the player is moving 
            if (body.velocity.y < 0)
            {
                
            }
            anim.SetFloat("velocityY", 1 * Mathf.Sign(body.velocity.y));
        }

        if (IsGrounded())
        {
            anim.SetBool("grounded", true);
            anim.SetFloat("velocityY", 0);
        }
        
        if (currentHealth<=0)
        {
            anim.SetTrigger("death");
            currentHealth = maxHealth;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentHealth -= 20; 
            healthBar.SetHealthBar(currentHealth);
        }

        
    }

 
}
