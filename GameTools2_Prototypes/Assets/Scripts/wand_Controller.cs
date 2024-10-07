using System;
using UnityEditor;
using UnityEngine;

public class wand_Controller : MonoBehaviour
{
    [Header("Fire Wand")]
    public GameObject fire_Wand;
    
    [Header("Ice Wand")]
    public GameObject ice_Wand;
    
    [Header("Electric Wand")]
    public GameObject electric_Wand;
    
    [Header("Transmutation Wand")]
    public GameObject transmutation_Wand;


    private int current_Wand;
    
    private void Start()
    {
        fire_Wand.SetActive(true);
        ice_Wand.SetActive(false);
        electric_Wand.SetActive(false);
        transmutation_Wand.SetActive(false);
    }// end Start()

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot_Primary_Spell();
        }
        
        Swap_Wand();
        
    }// end Update()

    private void Shoot_Primary_Spell()
    {
        if (fire_Wand.activeSelf)
        {
            print("shooting fire wand");
        }

        else if (ice_Wand.activeSelf)
        {
            print("shooting ice wand");
        }

        else if (electric_Wand.activeSelf)
        {
            print("shooting electric wand");
        }
        
        else if (transmutation_Wand.activeSelf)
        {
            print("shooting trans wand");
        }
    }// end Shoot_Primary_Spell()

    private void Swap_Wand()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("swap to fire wand");
            fire_Wand.SetActive(true);
            ice_Wand.SetActive(false);
            electric_Wand.SetActive(false);
            transmutation_Wand.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("swap to ice wand");
            fire_Wand.SetActive(false);
            ice_Wand.SetActive(true);
            electric_Wand.SetActive(false);
            transmutation_Wand.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("swap to electric wand");
            fire_Wand.SetActive(false);
            ice_Wand.SetActive(false);
            electric_Wand.SetActive(true);
            transmutation_Wand.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            print("swap to trans wand");
            fire_Wand.SetActive(false);
            ice_Wand.SetActive(false);
            electric_Wand.SetActive(false);
            transmutation_Wand.SetActive(true);
        }
        
    }// end Swap_Wand()

}// end script
