using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    //  Public variables ---------------------------------------------------------

    /// <summary>
    /// The Controllor for Healthpoints, if the character is alive, and the
    /// life-bar of it
    /// </summary>    
    public HealthController healthController;
    /// <summary>
    /// The amount of damage the character deals
    /// </summary>
    
    public int damage;
    /// <summary>
    /// The attacking rqange of the character
    /// </summary>
    public int attackRange;

    /// <summary>
    /// A reference to the GameLogic
    /// </summary>
    public GameLogic gameLogic;

    /// <summary>
    /// A Reference to the rigidbody
    /// </summary>
    public Rigidbody2D rigidBody;
    public Animator anim;

    public LayerMask groundLayer;
	public Transform groundBack;
    public Transform groundFront;
    
    /// <summary>
    /// The Speed the character moves
    /// </summary>
    public float movementSpeed;

    /// <summary>
    /// Flag if the character is facing to the left
    /// </summary>
    public bool directionLeft = false;

    /// <summary>
    /// The Time in seconds between the next attack
    /// </summary>
    public float attackSpeed = 0.5F;

    /// <summary>
    /// An Flag if the character is allowed to attack
    /// </summary>
    public bool isAllowedToAttack = true;
    public bool isGroundedBack = false;
    public bool isGroundedFront = false;

    // --------------------------------------------------------------------------------------

    /// <summary>
    /// The Time when the character is allowed to attack again
    /// </summary>
    protected float nextAttack = 0.0f;

	public virtual void Start () {
        
        Debug.Log("Start Character");
        init();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		isAllowedToAttack = getIsAllowedToAttack();
		isGroundedBack = getIsGroundedBack();
		isGroundedFront = getIsGroundedFront();
	}

    public void init() {
        healthController = GetComponent<HealthController>();
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();        
    }

    public void move() {
        //  Check if character has to flip
        if (!isGroundedFront) {
            flipDirection();
        }

        //  Move
        anim.SetBool("move", true);
        rigidBody.velocity = directionLeft ? 
            new Vector2(-movementSpeed, rigidBody.velocity.y) 
            : new Vector2(movementSpeed, rigidBody.velocity.y);  
    }

    /// <summary>
    /// Flips the direction. Sets the parameter 'directionLeft'
    /// </summary>
    public void flipDirection() {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        directionLeft = !directionLeft;
    }

    private bool getIsAllowedToAttack() {        
		return Time.time > nextAttack;
	}

    public bool getIsDirectionLeft() {
        return directionLeft;
    }

	private bool getIsGroundedBack() {
		Collider2D[] collidersBack = Physics2D.OverlapCircleAll(groundBack.position, 0.2f, groundLayer);
        for (int i = 0; i < collidersBack.Length; i++) {
            if (collidersBack[i].gameObject != gameObject) {
                return true;                
            }
        }
		return false;
	}	

	private bool getIsGroundedFront() {
        
		Collider2D[] collidersBack = Physics2D.OverlapCircleAll(groundFront.position, 0.2f, groundLayer);
        for (int i = 0; i < collidersBack.Length; i++) {
            if (collidersBack[i].gameObject != gameObject) {
                
                return true;                
            }
        }
		return false;
	}	
}
