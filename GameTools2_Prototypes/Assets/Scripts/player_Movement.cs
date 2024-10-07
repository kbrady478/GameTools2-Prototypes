// tutorial used: https://www.youtube.com/watch?v=f473C43s8nE

using System.Collections;
using UnityEngine;

public class player_Movement : MonoBehaviour
{
    [Header("General")]
    public GameObject body_GFX;
    public Transform orientation;
    public Transform camera_Direction;
    public Rigidbody rigid_Body;
    public float max_Velocity;

    private Vector3 move_Direction;
    private float horizontal_Input;
    private float vertical_Input;

    [Header("Keybinds")]
    public KeyCode jump_Key = KeyCode.Space;
    public KeyCode crouch_Key = KeyCode.LeftControl;

    [Header("Ground Check")]
    public LayerMask ground_Mask;
    public float player_Height;
    public bool is_Grounded;

    [Header("Walking")]
    public float move_Speed;
    public float ground_Drag;

    [Header("Jumping")]
    public float jump_Force;
    public float jump_Cooldown;
    public float air_Multiplier;
    public bool jump_Ready;

    [Header("Crouching")]
    public float crouch_Height;
    public float crouch_Width;
    public float crouch_Transition_Time;
    public bool is_Crouched;

    private Vector3 standing_Scale;
    private Vector3 crouch_Scale;
    private bool has_Crouch_Scaled;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;         
        
        rigid_Body.freezeRotation = true;

        jump_Ready = true; 
        has_Crouch_Scaled = false;


        // need to do jumping scale
        standing_Scale = new Vector3(body_GFX.transform.localScale.x, body_GFX.transform.localScale.y, body_GFX.transform.localScale.z);
        crouch_Scale = new Vector3(crouch_Width, crouch_Height, crouch_Width);
    }

    private void Update()
    {        
        // ground check raycast, half the player height and then some to make contact
        is_Grounded = Physics.Raycast(transform.position, Vector3.down, (player_Height * body_GFX.transform.localScale.y) * 0.5f + 0.2f, ground_Mask);

        Player_Input();
        Speed_Control();

        // apply drag
        if (is_Grounded)
            rigid_Body.linearDamping = ground_Drag;

        else
            rigid_Body.linearDamping = 0;

    }// end Update()

    private void FixedUpdate()
    {
        //Respawn();
        if (rigid_Body.angularVelocity.magnitude > max_Velocity)
            rigid_Body.angularVelocity = Vector3.ClampMagnitude(rigid_Body.angularVelocity, max_Velocity);
        
        Move_Player();
    }// end FixedUpdated()
/*
    private void Respawn()
    {
        if (rigid_Body.transform.position.y < respawn_Layer.transform.position.y)
        {
            rigid_Body.transform.position = respawn_Point.transform.position;
        }
    }
*/
/// BASE MOVEMENT FUNCTIONS 


    private void Player_Input()
    {
        horizontal_Input = Input.GetAxisRaw("Horizontal");
        vertical_Input = Input.GetAxisRaw("Vertical");

        // crouching
        if (Input.GetKeyUp(crouch_Key))
        {
            is_Crouched = false;
            has_Crouch_Scaled = false;
            StopCoroutine("Stand_to_Crouch_Transition");
            StartCoroutine("Crouch_to_Stand_Transition");
        }
        else if (Input.GetKey(KeyCode.LeftControl) && is_Grounded)
            Crouching();
            
        // jumping
        if (Input.GetKey(jump_Key) && jump_Ready && is_Grounded && !is_Crouched)
        {
            jump_Ready = false;

            Jump();

            Invoke(nameof(Reset_Jump), jump_Cooldown);
        }

    }// end Player_Input()
    
    private void Move_Player()
    {
        // calculate direction
        move_Direction = orientation.forward * vertical_Input + orientation.right * horizontal_Input;

        if (is_Grounded)
            rigid_Body.AddForce(move_Direction.normalized * move_Speed * 10f * Time.deltaTime, ForceMode.Force); // extra 10f because its slow without

        else if (!is_Grounded)
            rigid_Body.AddForce(move_Direction.normalized * move_Speed * 5f  * Time.deltaTime, ForceMode.Force); 

    }// end Move_Player()

    private void Speed_Control()
    {
        Vector3 flat_Velocity = new Vector3(rigid_Body.angularVelocity.x, 0f, rigid_Body.angularVelocity.z);
        
        // limit velocity when needed
        if (flat_Velocity.magnitude > move_Speed)
        {
            Vector3 limited_Velocity = flat_Velocity.normalized * move_Speed;
            rigid_Body.angularVelocity = new Vector3(limited_Velocity.x, rigid_Body.angularVelocity.y, limited_Velocity.z);
        }
    
    }// end Speed_Control()


/// JUMPING FUNCTIONS


    private void Jump()
    {
        // reset Y velocity
        rigid_Body.angularVelocity = new Vector3(rigid_Body.angularVelocity.x, 0f, rigid_Body.angularVelocity.z);

        rigid_Body.AddForce(transform.up * jump_Force, ForceMode.Impulse); // ForceMode.Impulse to only apply once

    }// end Jump()

    private void Reset_Jump()
    {
        jump_Ready = true;
    }// end Reset_Jump

    
/// CROUCHING + BULLET JUMP FUNCTIONS


    private void Crouching()
    {
        is_Crouched = true;
        
        if (has_Crouch_Scaled == false)
        {
            has_Crouch_Scaled = true;
            StopCoroutine("Crouch_to_Stand_Transition");
            StartCoroutine("Stand_to_Crouch_Transition");
            
        }

    }// end Crouching()

    IEnumerator Stand_to_Crouch_Transition()
    {
        float time_Elapsed = 0;

        while (time_Elapsed < 1)
        {
            body_GFX.transform.localScale = Vector3.Lerp(body_GFX.transform.localScale, crouch_Scale, time_Elapsed);
            time_Elapsed += Time.deltaTime * crouch_Transition_Time;

            yield return null;
        }

    }// end Stand_to_Crouch_Transition()

    IEnumerator Crouch_to_Stand_Transition()
    {
        float time_Elapsed = 0;

        while (time_Elapsed < 1)
        {
            body_GFX.transform.localScale = Vector3.Lerp(body_GFX.transform.localScale, standing_Scale, time_Elapsed);
            time_Elapsed += Time.deltaTime * (crouch_Transition_Time *0.5f);

            yield return null;
        }

    }// end Crouch_to_Stand_Transition()


 
}// end script
