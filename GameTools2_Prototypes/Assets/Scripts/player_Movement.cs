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

    [Header("Bullet Jumping")]
    public float bullet_Jump_Force;
    public float bullet_Jump_Charge;
    public float bullet_Jump_Drag;
    public bool is_Bullet_Jumping;

    [Header("Bullet Jump Animation")]
    public float twirl_Duration;
    public float sideways_Duration;

    private Quaternion twirl_Start;
    private Quaternion twirl_Target;
    private Quaternion upright_Rotation;
    private Quaternion sideways_Rotation;

    // could be an idea where you chain bullet jumps by removing ground check on crouch, instead put it on the bullet jump check
    // allows for jumps to be charged midair for release on contact with ground

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;         
        
        rigid_Body.freezeRotation = true;

        jump_Ready = true; 
        has_Crouch_Scaled = false;
        is_Bullet_Jumping = false;

        // need to do jumping scale
        standing_Scale = new Vector3(body_GFX.transform.localScale.x, body_GFX.transform.localScale.y, body_GFX.transform.localScale.z);
        crouch_Scale = new Vector3(crouch_Width, crouch_Height, crouch_Width);

        // to twirl character gfx on local y
        twirl_Start = body_GFX.transform.rotation;
        twirl_Target = twirl_Start * Quaternion.Euler(0, 360, 0);

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
        Move_Player();
    }// end FixedUpdated()


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
            rigid_Body.AddForce(move_Direction.normalized * move_Speed * 10f *air_Multiplier * Time.deltaTime, ForceMode.Force); 

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

        // Bullet Jump
        if (Input.GetKey(jump_Key))
        {
            rigid_Body.AddForce(rigid_Body.transform.up * (jump_Force * 0.25f), ForceMode.Impulse);
            rigid_Body.AddForce(camera_Direction.transform.forward * (bullet_Jump_Force * bullet_Jump_Charge), ForceMode.Impulse); // ForceMode.Impulse to only apply once
            rigid_Body.linearDamping = bullet_Jump_Drag;
            bullet_Jump_Charge = 0f; // reset after jump

           //StartCoroutine("Bullet_Jump_Alignment");
        }
        
        // Crouching
        else if (has_Crouch_Scaled == false)
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
            bullet_Jump_Charge += time_Elapsed;

            if (bullet_Jump_Charge > 1)
                bullet_Jump_Charge = 1f;

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
            bullet_Jump_Charge -= time_Elapsed;

            if (bullet_Jump_Charge < 0)
                bullet_Jump_Charge = 0f;

            yield return null;
        }

    }// end Crouch_to_Stand_Transition()

    // align character rotation with angle of camera to give leaping effect
    
    // change the x of the body_GFX to 90 then back
   /*
    IEnumerator Bullet_Jump_Alignment()
    {
        upright_Rotation = Quaternion.Euler(body_GFX.transform.localRotation.x, rigid_Body.transform.rotation.y, rigid_Body.transform.rotation.z);
        sideways_Rotation = Quaternion.Euler(body_GFX.transform.localRotation.x + 50, rigid_Body.transform.rotation.y, rigid_Body.transform.rotation.z);

        StopCoroutine("Stand_to_Crouch_Transition");
        StopCoroutine("Crouch_to_Stand_Transition");

        float time_Elapsed = 0;

        while (time_Elapsed < (1))
        {
            body_GFX.transform.localRotation = Quaternion.Slerp(upright_Rotation, sideways_Rotation, time_Elapsed);
            time_Elapsed += Time.deltaTime * sideways_Duration;

            // if done sideways do upright

            yield return null;
        }
        
    }// end Bullet_Jump_Alignment()

    /*
    IEnumerator Bullet_Jump_Twirl()
    {
        float time_Elapsed = 0;

        while (time_Elapsed < 1)
        {
            body_GFX.transform.rotation = Quaternion.Slerp(twirl_Start, twirl_Target, time_Elapsed);
            time_Elapsed += Time.deltaTime * twirl_Duration;

            yield return null;
        }


    }// end Bullet_Jump_Anim()
    */

}// end script
