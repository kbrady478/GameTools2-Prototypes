// Tutorial used for patrolling state: https://www.youtube.com/watch?v=vS6lyX2QidE&t=238s

//using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Enemy_Movement : MonoBehaviour, IFireball, IIcicle, IElectroball, ITransmutation
{
    [Header("General")]
    public Color regular_Color;

    private Collider obj_Collider;
    
    [Header("Enemy Navigation")]
    public Transform[] patrol_Points; // Array of patrol waypoints, added in editor
    public int target_Point; // Next patrol point to walk to
    
    private NavMeshAgent enemy_Nav_Agent;
    private Rigidbody rb;
    private bool can_Move;
    
    [Header("Knockback Effect")]
    public LayerMask ground_layer;

    private bool is_Grounded;

    
    [Header("Frozen Effect")]
    public Material enemy_Material;
    public Color  frozen_Color; 
    public float frozen_Time;
    
    private float regular_Speed;   
    
    
    [Header("Electrfied Effect")]
    public Color electrocuted_Color;
    public GameObject electric_Effect;
    public float electrify_Time;


    [Header("Transmutation Effect")]
    public GameObject player_GFX;
    public GameObject[] transmutation_Items;
    
    private GameObject transmutation;
    private int current_Item;
    
    
    private void Start()
    {
        obj_Collider = GetComponent<Collider>();
        
        enemy_Nav_Agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        target_Point = 0;
        
        regular_Speed = enemy_Nav_Agent.speed;
        enemy_Material.color = regular_Color;

        electric_Effect.SetActive(false);
        can_Move = true;
    }// end Start

    private void Update()
    {
        if (can_Move)
            Patrol_State();
    }// end Update
    
    private void Patrol_State()
    {
        float distance_To_Waypoint = Vector3.Distance(patrol_Points[target_Point].position, transform.position);

        // Checks if close enough to target waypoint, then changes to next
        if (distance_To_Waypoint <= 3)
        {
            // put timer here **************
            target_Point = (target_Point + 1) % patrol_Points.Length;
        }
        
        enemy_Nav_Agent.SetDestination(patrol_Points[target_Point].position);

    }// end Patrol_State

    // FIREBALL EFFECTS
    
    public void disable_Nav()
    {
        print("nav disabled");
        can_Move = false;
        enemy_Nav_Agent.enabled = false;
    }
    
    // ICICLE EFFECTS
    
    public void Is_Frozen()
    {
        StopCoroutine(Frozen());
        print("slow recieved");
        enemy_Nav_Agent.speed = enemy_Nav_Agent.speed * 0.25f;
        enemy_Material.color = frozen_Color;
        StartCoroutine(Frozen());
    }

    private IEnumerator Frozen()
    {
        yield return new WaitForSeconds(frozen_Time);
        
        enemy_Nav_Agent.speed = regular_Speed;
        enemy_Material.color = regular_Color;
    }
    
    // ELECTRIC EFFECTS

    public void Is_Electrocuted()
    {
        StopCoroutine(Electrified());
        print("electrocuted");
        
        electric_Effect.SetActive(true);
        enemy_Nav_Agent.enabled = false;
        can_Move = false;
        enemy_Material.color = electrocuted_Color;
        
        StartCoroutine(Electrified());
    }

    private IEnumerator Electrified()
    {
        yield return new WaitForSeconds(electrify_Time);

        electric_Effect.SetActive(false);
        enemy_Nav_Agent.enabled = true;
        can_Move = true;
        enemy_Material.color = regular_Color;
    }
    
    // TRANSMUTATION EFFECT 

    public void Transmutate()
    {
        can_Move = false;
        
        if (transmutation != null)
            transmutation.SetActive(false);
        
        int item = Random.Range(0, transmutation_Items.Length);
        while (item == current_Item)
            item = Random.Range(0, transmutation_Items.Length);
        
        enemy_Nav_Agent.enabled = false;
        rb.isKinematic = false;
        obj_Collider.enabled = false;
        player_GFX.SetActive(false);
        
        transmutation = transmutation_Items[item];
        transmutation.SetActive(true);
    }
    
}// end Enemy_Movement


    