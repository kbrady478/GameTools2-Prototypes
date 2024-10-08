using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

interface IFireball
{
    void disable_Nav();
}


public class fireball : MonoBehaviour
{
    private reset_Enemy reset_Enemy_Script;
    private GameObject target;

    
    [FormerlySerializedAs("block_Layer")] [Header("Explosion Effect")] 
    public LayerMask rigid_Body_Layer;
    public int hit_Limit;
    public float radius;
    public float explosion_Force;

    private Collider[] hit_Colliders;
    private bool knockback_Completed;

    
    private void Awake()
    {
        hit_Colliders = new Collider[hit_Limit];
        knockback_Completed = false;
        target = GameObject.FindGameObjectWithTag("Target");
        reset_Enemy_Script = target.GetComponent<reset_Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("fireball exploding");
        if (other.CompareTag("Enemy"))
        {
            print("found enemy");
            IFireball enemy = other.GetComponent<IFireball>();
            if (enemy != null)
            {
                print("disabling enemy nav");
                enemy.disable_Nav();
            }
        }
        else if(other.CompareTag("Target"))
            reset_Enemy_Script.Reset();
        
        Knockback();
        
        if (knockback_Completed)
            Destroy(gameObject);
    }

    private void Knockback()
    {
        print("enter knockback");
        
        int colliders_Hit_Count = Physics.OverlapSphereNonAlloc(transform.position, radius, hit_Colliders, rigid_Body_Layer);

        for (int i = 0; i < colliders_Hit_Count; i++)
        {
            print("attempting to find rigidbody");
            
            Rigidbody rb = hit_Colliders[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                print("attempting to knockback: " + rb.gameObject.name);
                rb.AddExplosionForce(explosion_Force, transform.position, radius);
                
            }
        }
        
        knockback_Completed = true;
    }
}
