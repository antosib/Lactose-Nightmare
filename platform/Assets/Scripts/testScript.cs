using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testScript : MonoBehaviour
{
    //velocita personaggio
    [SerializeField] public float speed;

    //forza applicata nel salto
    [SerializeField] private float jumpForce;

    //layer contenente tutti gli oggetti che rappresentano il ground
    [SerializeField] private LayerMask groundLayer;

   

    //layer contenente tutti gli oggetti che rappresentano il wall
    // [SerializeField] private LayerMask wallLayer;

    //riferimento alla barra della vita
  

    //flag per controllare se il player si rivolge verso destra
    private bool facingRight = true;

    //gestisce le  animazioni
    public Animator anim;

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
    private bool attack;
    private bool isInvincible;
    [SerializeField]
    private float invincibilityDurationSeconds;

    private Vector2 rollDir;
    private float rollSpeed;

    public Transform attackPoint;
    [SerializeField] private float attackRange;
    public LayerMask enemyLayers;
    public static testScript instance;
    public HealthSystem healthSystem;
    public int combo;
    
    public GameMaster gm;




    public enum State
    {
        normal,
        rolling,
        attacking,
        death
    }




    public State state;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
      
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        state = State.normal;
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
        isInvincible = false;


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


                if (rollSpeed < 1f)
                {

                    state = State.normal;

                }
                
              
                break;


            case State.attacking:
                anim.SetBool("run",false);
                if (!attack)
                {
                    state = State.normal;
                    
                }

                break;
            
            case State.death:
                if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                run = xInput != 0;


                body.velocity = new Vector2(xInput * speed, body.velocity.y);




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

                body.velocity = rollSpeed * rollDir;


                break;
            case State.attacking:
                body.velocity = Vector2.zero;
                break;
            
            case State.death:
               
               
                break;


        }

    }

    private void Jump()
    {
        //ForceMode2D.Impulse is useful if Jump() is called using GetKeyDown
        body.velocity = new Vector2(body.velocity.x, jumpForce);

        anim.SetBool("try", true);
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
        RaycastHit2D raycast = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down,
            0.1f, groundLayer);
        return raycast.collider != null;
    }

    private void HandleAnimations()
    {
        anim.SetBool("run", run);
        
        if (IsGrounded())
        {
            anim.SetBool("grounded", true);


        }
        else
        {
            anim.SetBool("grounded", false);

            //Set the animator velocity equal to 1 * the vertical direction in which the player is moving 
            if (body.velocity.y < 0)
            {
                anim.SetBool("try", false);
                anim.SetTrigger("fall");
            }

        }

        if (healthSystem.GetCurrentHealth() <= 0)
        {
            anim.SetTrigger("death");
            state = State.death;
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


        if (jumpBufferCounter > 0 && hangTimeCounter > 0)
        {
            jump = true;
        }

        if (Input.GetMouseButtonDown(0) && grounded && !attack)
        {
            anim.SetTrigger(""+combo);
            OnClick();
            attack = true;
            state = State.attacking;

        }

        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftArrow) && grounded)
        {
            anim.SetTrigger("roll");
            rollDir = Vector2.left;
            rollSpeed = speed;
            state = State.rolling;
            TriggersInvulnerability();


        }


        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.RightArrow) && grounded)
        {
            anim.SetTrigger("roll");

            rollDir = Vector2.right;
            state = State.rolling;
            rollSpeed = speed;
            TriggersInvulnerability();

        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnClick()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("ho hittato un" + enemy.name);
        }


    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    public bool IsAttacking()
    {
        return attack;
    }

    public bool IsRunning()
    {
        return run;
    }

    public void StartCombo()
    {
        attack = false ;
        if (combo < 3)
        {
            combo++;
        }
    }


    public void FinishAnim()
    {
        attack = false;
        combo = 0;
    }
    
    
    private IEnumerator BecomeTemporarilyInvincible()
    {
        Debug.Log("Player turned invincible!");
        isInvincible = true;
        
        yield return new WaitForSeconds(invincibilityDurationSeconds);

        isInvincible = false;
        Debug.Log("Player is no longer invincible!");
    }
    
    void TriggersInvulnerability()
    {
        if (!isInvincible)
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

}
