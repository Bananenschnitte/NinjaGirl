using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiMelee : Character
{
	public float sightRange;
	public GameObject target;

    
    private bool isFacingToPlayer = false;
	private bool isTargetInSight = false;
	private bool isTargetInAttackingRange = false;
	
	

	private float distanceToTarget = 0f;
	



	// Use this for initialization
	public override void Start () {
        base.Start();    
        Debug.Log("Start KiMelee");
	}
	
	// Update is called once per frame
	public override void Update () {
        //Debug.Log("Update KiMelee");
        base.Update();
	    getInputs();
		makeDecision();
	}

	/// <summary>
	/// Checks the enviroment of the this Character and gets all the "is"-Parameters
	/// </summary>
	private void getInputs() {
		distanceToTarget = getDistanceToTarget();
		isFacingToPlayer = getIsFacingToPlayer();
		isTargetInSight = getIsTargetInSight();
		isTargetInAttackingRange = getIsTargetInAttackRange();
	}

	/// <summary>
	/// Makes the decision what this character should do now.
	/// Priority of doing:
	/// 	1	Attack
	/// 	2	Walk
	/// </summary>
	private void makeDecision() {
		if (isAllowedToAttack && isTargetInAttackingRange && isFacingToPlayer) {
			attack();
		} else if (!isAllowedToAttack) {
            // do nothing
        } else {			
			move();
		}
	}

	private bool getIsFacingToPlayer() {
		if (this.transform.position.x > target.transform.position.x) {
			return directionLeft;
		} else {
			return !directionLeft;
		}
	}

	private bool getIsTargetInSight() {		
		return distanceToTarget >= sightRange;
	}
    
	private float getDistanceToTarget() {
		return Mathf.Abs(transform.position.x - target.transform.position.x);
	}

	private bool getIsTargetInAttackRange() {
		return distanceToTarget < attackRange;
	}

    public void attack() {
        anim.SetTrigger("attack");
        // may set last attack time, to stop character from doing anything other
        HealthController hc = target.GetComponent<HealthController>();
        hc.takeDamage(damage);
        nextAttack = Time.time + attackSpeed;
    }
}
