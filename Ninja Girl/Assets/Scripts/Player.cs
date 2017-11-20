using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]    private float slideForce = 1.0f;
    [SerializeField]    private float jumpForce = 1.0f;
    [SerializeField]    private float groundRadius = 0.2f;
    [SerializeField]    private Transform[] groundPoints;
    [SerializeField]    private float movementSpeed;
    [SerializeField]    private LayerMask groundLayer;
    // [SerializeField]    private LayerMask layerEnemys;
    [SerializeField]    private float rigidbodyGravityScaleNormal = 1.0f;
    [SerializeField]    private float rigidbodyGravityScaleGliding = 0.5f;
    [SerializeField]    private BoxCollider2D weaponRangeCollider;

    private int layerGround = 9;
    public HealthController healthController;    


    private bool facingRight = true;
    private float lastJumpTime = 0f;

    /// <summary>
    /// The Rigidbody of the character
    /// </summary>
    private Rigidbody2D rigidbody;

    /// <summary>
    /// Input if the character should move left or right
    /// </summary>
    private float horizontal;

    private bool attack = false;
    private bool jump = false;
    private bool slide = false;
        
    private bool isAttacking = false;
    private bool isSliding = false;
    private bool isGrounded = false;
    private bool isGliding = false;
    private bool isAlive = true;
    
    private Animator anim;

    // -------------------------------------------------------------------

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        layerGround = LayerMask.NameToLayer("Ground");
        healthController = GetComponent<HealthController>();
        Debug.Log("Layer:" + layerGround + " / groundlayeR: "+ groundLayer.GetHashCode() );
	}

    void Update()
    {
        //  read input
        handleInput();
              
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isAlive)
        {
            getCurrentState();
            handleMovement();
            handleAttacks();
            setIsGrounded();
            setAnimatorParameters();
            checkLiveState();
        }
        
	}

    private void handleAttacks()
    {
        if (!isSliding 
            && attack 
            && isGrounded)
        {            
            //  Stop character to move
            rigidbody.velocity = Vector2.zero;
        }

        //  Check if enemya is hit
    }

    private GameObject[] getHitTargets()
    {
        // if (weaponRangeCollider.IsTouchingLayers(layerEnemys))
        // {
            
        // }
        
        return null;
    }

    private void getCurrentState()
    {
        isSliding = anim.GetCurrentAnimatorStateInfo(0).IsName("Slide");
        isGliding = anim.GetCurrentAnimatorStateInfo(1).IsName("Glide");
        isAttacking = getIsAttacking();

    }

    private void handleInput()
    {
        horizontal = Input.GetAxis("Horizontal");        

        //attack = Input.GetKeyDown(KeyCode.Space);
        attack = Input.GetKeyDown(KeyCode.Space);

        //jump = Input.GetKeyDown(KeyCode.UpArrow);        
        jump = Input.GetKey(KeyCode.UpArrow);        

        slide = Input.GetKey(KeyCode.DownArrow);
        
    }

    private void handleMovement()
    {
        if (isSliding) return;
        if (isAttacking && isGrounded) return;

        //anim.SetBool("isGrounded", isGrounded);

        //  move
        rigidbody.velocity = new Vector2(horizontal * movementSpeed, rigidbody.velocity.y);    
    
        //  Glide-Modification
        glide();
                
        //  jump
        movementJump();
                
        // flip
        flip();

        //  slide
        if (isGrounded)
        {
            movementSlide();
        }
        
    }

    private void movementJump()
    {
        
        if (lastJumpTime + 0.5 > Time.time)
        {
            return;
        }

        if (isGrounded && jump)
        {
            anim.SetTrigger("jump");
            lastJumpTime = Time.time;
            isGrounded = false;
            //rigidbody.AddForce(new Vector2(0, jumpForce));
            addForceVertical(jumpForce);

        }
    }

    private void glide()
    {
        if (isGliding)
        {
            rigidbody.gravityScale = rigidbodyGravityScaleGliding;
        }
        else
        {
            rigidbody.gravityScale = rigidbodyGravityScaleNormal;
        }
    }

    private void movementSlide()
    {
        if (!anim.GetBool("slide") && slide)
        {
            addForceHorizontal(slideForce);
            isSliding = true;
        }
    }

    public void addForceHorizontal(float force)
    {
        if (facingRight)
        {
            rigidbody.AddForce(new Vector2(force, 0));
        }
        else
        {
            rigidbody.AddForce(new Vector2(force * -1, 0));
        }
    }

    public void addForceVertical(float force)
    {
        rigidbody.AddForce(new Vector2(0, force));
    }

    private void setAnimatorParameters()
    {
        anim.SetFloat("speed", Mathf.Abs(horizontal));

        //if (jump) anim.SetTrigger("jump");
        //anim.SetTrigger("jump");

        anim.SetBool("slide", slide);
        if (attack && !isAttacking) {
            anim.SetTrigger("attack");
        }
        anim.SetBool("isGrounded", isGrounded);
    }

    private void flip()
    {
        
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            facingRight = !facingRight;
            
        }
    }

    private void setIsGrounded() {
        if (isSliding)
        {
            isGrounded = true;
            return;
        }

        if (rigidbody.velocity.y <= 0) {
            foreach (Transform point in groundPoints)
            {                
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, groundLayer );
                for (int i = 0; i < colliders.Length; i++)
                {                                        
                    if (colliders[i].gameObject != gameObject)
                    {
                        isGrounded = true;                       
                        return;                        
                    }
                }
            }
        }
        isGrounded = false;
    }

    private bool getIsAttacking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return true;
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Jump_Attack")) return true;
        return false;
    }
       
    private void checkLiveState()
    {
        if (!healthController.isAlive())
        {               
            this.rigidbody.velocity = Vector3.zero;
            anim.SetTrigger("die");
            isAlive = false;
            Debug.Log("Player died !");
            this.rigidbody.simulated = false;
            
        }
    }
}
