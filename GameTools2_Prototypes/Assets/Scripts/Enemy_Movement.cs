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

public class Enemy_Movement : MonoBehaviour, IFireball, IIcicle, IElectroball
{
    [Header("General")]
    public Color regular_Color;
    
    
    [Header("Enemy Navigation")]
    private NavMeshAgent enemy_Nav_Agent;
    private Rigidbody rb;
    private bool can_Move;
    
    public Transform[] patrol_Points; // Array of patrol waypoints, added in editor
    public int target_Point; // Next patrol point to walk to
   
    
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

    private bool is_Electrified;
    
    
    private void Start()
    {
        enemy_Nav_Agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        target_Point = 0;
        
        regular_Speed = enemy_Nav_Agent.speed;
        enemy_Material.color = regular_Color;

        electric_Effect.SetActive(false);
        is_Electrified = false;
    }// end Start

    private void Update()
    {
        if (is_Electrified == false)
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
        can_Move = false;
        enemy_Nav_Agent.enabled = false;
    }

    private void Renable_Nav()
    {
        
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
        is_Electrified = true;
        enemy_Material.color = electrocuted_Color;
        
        StartCoroutine(Electrified());
    }

    private IEnumerator Electrified()
    {
        yield return new WaitForSeconds(electrify_Time);

        electric_Effect.SetActive(false);
        enemy_Nav_Agent.enabled = true;
        is_Electrified = false;
        enemy_Material.color = regular_Color;
    }
}// end Enemy_Movement


    