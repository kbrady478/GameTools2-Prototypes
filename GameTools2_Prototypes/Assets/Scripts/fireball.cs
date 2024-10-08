using System;
using UnityEngine;

public class fireball : MonoBehaviour
{
    [Header("Explosion Effect")] 
    public LayerMask hit_Layer;
    public LayerMask block_Layer;
    public int max_Hits;
    public int max_Damage;
    public int min_Damage;
    public float radius;
    public float explosion_Force;

    private Collider[] hit_Colliders;

    private void Awake()
    {
        hit_Colliders = new Collider[max_Hits];
    }

    private void OnTriggerEnter(Collider other)
    {
        print("fireball exploding");
        
        if (other.CompareTag("Player"))
        {
            Physics.Raycast()
            int hits = Physics.OverlapSphereNonAlloc(other.transform.position, radius, hit_Colliders, hit_Layer);
        }
        
        Destroy(gameObject);
    }
}
