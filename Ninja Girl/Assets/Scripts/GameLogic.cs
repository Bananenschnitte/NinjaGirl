using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    [SerializeField]
    private GameObject explosion;

    private int ATTACKTYPE_Melee = 1;
    private int ATTACKTYPE_Range = 2;
    private int ATTACKTYPE_Magic = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void takeDamage(GameObject attacker, GameObject victim, int dmg)
    {
        takeDamage(attacker, victim, dmg, 1);
    }

    public void takeDamage(GameObject attacker, GameObject victim, int dmg, int attackType)
    {
        HealthController healthController = victim.GetComponent<HealthController>();
        if (healthController == null)
        {
            Debug.LogError("HealthController not found");
            return;
        }

        healthController.takeDamage(dmg);

        //if (attackType == ATTACKTYPE_Melee) {
            //makeExplosion(getOriginOfAttackMelee(attacker.transform, victim.transform));
        makeExplosion(attacker.transform);
        //}

    }

    /// <summary>
    /// Creates an Explosion
    /// </summary>
    /// <param name="origin">Where the explosion should happen</param>
    public void makeExplosion(Transform origin)
    {        
        //Instantiate(explosion, origin);
        Instantiate(explosion, origin.position, origin.rotation);
    }

    public Transform getOriginOfAttackMelee(Transform attacker, Transform victim) {
        float x = victim.position.x - ((victim.position.x - attacker.position.x) / 2);
        //Transform t = new Transform();
        return null;

        //return new Vector2(x, victim.position.y);
    }

    public void attack(Character attacker, Character target)
    {
        target.healthController.takeDamage(attacker.damage);
    }

    public void attack(GameObject attacker, GameObject target)
    {
        Character targetFigure = target.GetComponent<Character>();
        Character attackerFigure = attacker.GetComponent<Character>();
        attack(attackerFigure, targetFigure);
    }

    /// <summary>
    /// Verifies if the target is in range
    /// </summary>
    /// <param name="source">the source that wants to attack</param>
    /// <param name="target">the target to attack</param>
    /// <param name="range">the range of the attack</param>
    /// <returns>Returns TRUE if the target is in range, returns false otherwise</returns>
    public bool isInRange(GameObject source, GameObject target, double range)
    {        
        float distance = Vector3.Distance(source.transform.position, target.transform.position);
        if (range >= distance)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Spawns/Creates an Object
    /// </summary>
    /// <param name="prefab">The Object to Create/Spawn</param>
    /// <param name="spawnTransform">Where to Spawn and which direction to point at (rotation)</param>
    public void spawnPrefab(GameObject prefab, Transform spawnTransform)
    {
        Instantiate(prefab, spawnTransform.position, spawnTransform.rotation);
    }

}
