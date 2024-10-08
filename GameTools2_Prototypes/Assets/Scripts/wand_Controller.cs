using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class wand_Controller : MonoBehaviour
{
    [Header("General")] 
    public Camera camera;
    public Transform casting_Point;
    public float projectile_Force;
    public float spell_Cooldown;
    public bool can_Fire; // Check if cooldown is over

    private bool allow_Invoke; // Ensure no repeats in single tap
    
    [Header("Fire Wand")]
    public GameObject fire_Wand;
    public GameObject fire_Projectile;
    
    [Header("Ice Wand")]
    public GameObject ice_Wand;
    public GameObject ice_Projectile;
    
    [Header("Electric Wand")]
    public GameObject electric_Wand;
    public GameObject electric_Projectile;
    
    [Header("Transmutation Wand")]
    public GameObject transmutation_Wand;
    public GameObject transmutation_Projectile;


    private int current_Wand;
    
    private void Start()
    {
        can_Fire = true;
        allow_Invoke = true;
        
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
        // Find point to shoot projectile at
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        // Check if ray hits
        Vector3 target_Point;
        if (Physics.Raycast(ray, out hit))
            target_Point = hit.point;
        else
            target_Point = ray.GetPoint(50); // When aiming at sky
        
        // Calculate direction
        Vector3 projectile_Direction = target_Point - casting_Point.position;
        
        if (can_Fire)
        {
            if (fire_Wand.activeSelf)
            {
                print("shooting fire wand");
                GameObject projectile = Instantiate(fire_Projectile, casting_Point.position, Quaternion.identity);
                projectile.transform.forward = projectile_Direction.normalized; 
                projectile.GetComponent<Rigidbody>().AddForce(projectile_Direction.normalized * projectile_Force, ForceMode.Impulse);
            }

            else if (ice_Wand.activeSelf)
            {
                print("shooting ice wand");
                GameObject projectile = Instantiate(ice_Projectile, casting_Point.position, Quaternion.identity);
                projectile.transform.forward = projectile_Direction.normalized; 
                projectile.GetComponent<Rigidbody>().AddForce(projectile_Direction.normalized * projectile_Force, ForceMode.Impulse);
            }

            else if (electric_Wand.activeSelf)
            {
                print("shooting electric wand");
                GameObject projectile = Instantiate(electric_Projectile, casting_Point.position, Quaternion.identity);
                projectile.transform.forward = projectile_Direction.normalized; 
                projectile.GetComponent<Rigidbody>().AddForce(projectile_Direction.normalized * projectile_Force, ForceMode.Impulse);
            }

            else if (transmutation_Wand.activeSelf)
            {
                print("shooting trans wand");
                GameObject projectile = Instantiate(transmutation_Projectile, casting_Point.position, Quaternion.identity);
                projectile.transform.forward = projectile_Direction.normalized; 
                projectile.GetComponent<Rigidbody>().AddForce(projectile_Direction.normalized * projectile_Force, ForceMode.Impulse);
            }

            if (allow_Invoke)
            {
                Invoke("Reset_Shot", spell_Cooldown);
                can_Fire = false;
                allow_Invoke = false;
            } 
        }
    }// end Shoot_Primary_Spell()

    private void Reset_Shot()
    {
        can_Fire = true;
        allow_Invoke = true;
    }// end Reset_Shot()
    
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
