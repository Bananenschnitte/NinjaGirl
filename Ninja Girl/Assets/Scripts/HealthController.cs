using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

    /// <summary> The GUI-Element to show the HealthPoints </summary>
    [SerializeField]
    private Slider slider;

    /// <summary> If the slider rotates to the camera </summary>
    private bool rotateSliderToCamera = true;

    /// <summary> The Healhtpoints the character starts with </summary>
    [SerializeField]
    private float startingHealth = 100f;

    /// <summary> The Color the fo the Healthbar with full HealthPoints </summary>
    [SerializeField]
    private Color fullHealthColor = Color.green;

    /// <summary> The Color the fo the Healthbar with no more HealthPoints </summary>
    [SerializeField]
    private Color zeroHealthColor = Color.red;

    /// <summary> The image component of the Slieder </summary>
    [SerializeField]
    private Image fillImage;

    /// <summary> The time of last damage. </summary>
    private System.DateTime timeOfLastCombatAction;


    private Camera mainCamera;


    /// <summary> The current Amount of Healthpoints</summary>
    [SerializeField]
    private float currentHealth;

    
    /// <summary> Indicator if the Character is alive </summary>
    private bool alive = true;


    public void Awake()
    {
        mainCamera = Camera.main;
    }


    public void Update()
    {
        drawHealthbar();
    }

    //	===================================================================================

    public void OnEnable()
    {
        currentHealth = startingHealth;
        alive = true;

        drawHealthbar();
    }

    //	===================================================================================


    /// <summary> Takes damage. </summary>
    /// <param name="amountOfDamage">Amount of damage.</param>
    public void takeDamage(int amountOfDamage)
    {

        currentHealth -= amountOfDamage;
        if (currentHealth <= 0)
        {
            alive = false;
        }

        drawHealthbar();
        timeOfLastCombatAction = System.DateTime.Now;

    }

    /// <summary> Update the Healthbar. (Setting value, and setting the Color of the bar) 
    /// Also rotates the healthbar to the camera </summary>
    private void drawHealthbar()
    {

        if (alive)
        {
            //	Set the sliedrs valure
            if (!slider)
            {
                Debug.LogError("Object ist alive. But Slider is not set");
                return;
            }

            if (currentHealth != slider.value)
            {
                //Debug.Log("Set Healthslider (slider.value=" + slider.value + ") (currentHealth=" + currentHealth + ")");
                slider.value = currentHealth;
                
                if (fillImage)
                {
                    fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startingHealth);
                }
            }

            if (rotateSliderToCamera)
            {
                slider.transform.rotation = mainCamera.transform.rotation;
            }
        }
        else
        {
            Debug.Log("Object is dead");
            if (slider != null)
            {
                if (slider.gameObject != null)
                {
                    slider.gameObject.SetActive(false);
                }
            }
        }

    }

    public bool isAlive()
    {
        return alive;
    }

    public void kill()
    {
        currentHealth = 0;
        alive = false;
    }
}
