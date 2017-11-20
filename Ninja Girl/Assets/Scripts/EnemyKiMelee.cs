using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiMelee : MonoBehaviour {

    public HealthController healthController;

    [SerializeField]    private float movementSpeed = 3.0f;
    [SerializeField]    private float weaponRange = 4;
    [SerializeField]    private float sightRange = 11;
    [SerializeField]    private Transform groundBack;
    [SerializeField]    private Transform groundFront;
    [SerializeField]    private GameObject targetPlayer;
    [SerializeField]    private LayerMask groundLayer;
    [SerializeField]    private int damage = 10;
    [SerializeField]    private float attackSpeed = 1.5f;
    [SerializeField]    private GameLogic gameLogic;

    private Rigidbody2D rigidbody;
    private Animator anim;
    [SerializeField]    private Player player;

    private bool isAttacking = false;
    private bool isAlive = true; //should be outsourced in healthmanagment
    private bool isPlayerInSight = false;
    private bool isPlayerInAttackRange = false;
    private bool isGroundedBack = false;
    private bool isGroundedFront = false;
    private bool isFacingToPlayer = false;
    private bool directionLeft = false;

    private float distanceToTarget = 9999f;
    private const short decision_attack = 1;
    private const short decision_walk = 2;
    private const short decision_charge = 3;
    private float lastAttackTime = 0;
    

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {

        //  Get Input of the Enviroment
        getInputs();

        //  Make decision what to do
        string decisionText = "";
        short decision = makeDecision();
        switch (decision)
        {
            case decision_attack:
                action_attack();
                decisionText = "Attack";
                break;

            case decision_walk:
                decisionText = "Walk";
                action_walk();
                break;

            default:
                decisionText = "Default";
                break;                
        }

        /*
        //  TEST
        Debug.Log("(Decision=" + decisionText + ") "
            + "(isAttacking?" + isAttacking + ") "
            + "(isPlayerInSight?" + isPlayerInSight + ") "
            + "(isPlayerInAttackRange?" + isPlayerInAttackRange + ") "
            + "(distanceToTarget=" + distanceToTarget +")" );
        */
	}

    /// <summary>
    /// Gets all the Inpurt, for the enemy.
    /// - Checks if the Player is in Sight.
    /// - Checks if the Player is in Weapon Range
    /// - Checks wich animation is played
    /// </summary>
    private void getInputs()
    {
        getDistanceToTarget();
        getCurrentState();
        getIsPlayerInSight();
        getIsPlayerInAttackRange();
        getIsGrounded();
        getIsFacingToPlayer();
    }

    private void getCurrentState() {
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack");                
    }

    /// <summary>
    /// Mea
    /// </summary>
    private void getDistanceToTarget()
    {        
        distanceToTarget = Vector3.Distance(transform.position, targetPlayer.transform.position);
    }

    /// <summary>
    /// Checks if the Player is in Range
    /// </summary>
    private void getIsPlayerInAttackRange()
    {
        isPlayerInAttackRange = distanceToTarget < weaponRange;        
    }

    /// <summary>
    /// Makes the decision for the enemy. 
    /// </summary>
    /// <returns>Return the decision-constant for the made decision</returns>
    private short makeDecision() {

        if (isPlayerInAttackRange && isFacingToPlayer && isAllowedToAttack())
        {
            return decision_attack;
        }

        //  Check if the Player is in Sight
        if (isPlayerInSight)
        {
        //    return decision_charge;
        }

        //  CHeck if the enemy has to idle
        //not implemented yet
        //return decision_idle;

        return decision_walk;
    }

    /// <summary>
    /// Finds out if the Player is in Sight
    /// </summary>
    private void getIsPlayerInSight()
    {
        isPlayerInSight = distanceToTarget < sightRange;
    }

    /// <summary>
    /// Action Definition that the enemy attacks the player.
    /// </summary>
    private void action_attack()
    {
        if (!isAttacking && isPlayerInAttackRange)
        {            
            anim.SetTrigger("attack");
            //player.healthController.takeDamage(damage);
            gameLogic.takeDamage(this.gameObject, player.gameObject, 25);
            lastAttackTime = Time.fixedTime;
        }
    }

    /// <summary>
    /// Checks if the Character is grounded to the left and to the right
    /// </summary>
    private void getIsGrounded()
    {
        isGroundedFront = false;
        isGroundedBack = false;        

        Collider2D[] collidersBack = Physics2D.OverlapCircleAll(groundBack.position, 0.2f, groundLayer);
        for (int i = 0; i < collidersBack.Length; i++)
        {
            if (collidersBack[i].gameObject != gameObject)
            {
                isGroundedBack = true;
                break; 
            }
        }

        Collider2D[] collidersFront = Physics2D.OverlapCircleAll(groundFront.position, 0.2f, groundLayer);
        for (int i = 0; i < collidersFront.Length; i++)
        {
            if (collidersFront[i].gameObject != gameObject)
            {
                isGroundedFront = true;
                break;
            }
        }

        //Debug.Log("(isGroundedLeft? " + isGroundedLeft + ") (isGroundedRight? " + isGroundedRight + ")");
    }

    /// <summary>
    /// Action DEfinition that the enemy walks.
    /// Also checks if the enemy has to flip the side
    /// </summary>
    private void action_walk()
    {
        //  check if enemy has to flip
        if (hasToFlip())
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            directionLeft = !directionLeft;
        }


        //  check if enemy has to idle (maybe in decisionmaking-methode)

        anim.SetBool("move", true);
        rigidbody.velocity = directionLeft ? 
            new Vector2(-movementSpeed, rigidbody.velocity.y) 
            : new Vector2(movementSpeed, rigidbody.velocity.y);    
    }

    /// <summary>
    /// Has the Character to flip?
    /// </summary>
    /// <returns>Return TRUE if the character has to flip, Returns False otherwise</returns>
    private bool hasToFlip()
    {
        //Debug.Log("(directionLeft?" + directionLeft + ") (isGroundedBack? " + isGroundedBack + ") (isGroundedFront? " + isGroundedFront + ")");
        if (!isGroundedFront && !isGroundedBack) return false;
        return !isGroundedFront;
    }

    /// <summary>
    /// Verifies if the character (enemy) is facing to the player.
    /// Sets the variable 'isFacingToPlayer'    
    /// </summary>
    private void getIsFacingToPlayer() {
        isFacingToPlayer = directionLeft ? (transform.position.x > targetPlayer.transform.position.x) : (targetPlayer.transform.position.x > transform.position.x);
    }

    private bool isAllowedToAttack()
    {
        if (lastAttackTime + attackSpeed < Time.fixedTime)
        {
            return true;
        }
        return false;
    }

}
