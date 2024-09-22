// tutorial used: https://www.youtube.com/watch?v=f473C43s8nE

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
    public float crouch_Width;
    public float crouch_Height;
    public float crouch_Transition_Time;
    public bool is_Crouched;

    private Vector3 crouch_Scale;

    // crouching will probably break ground check due to height, check may protrude down, need to fix

    private void Start()
    {
        rigid_Body.freezeRotation = true;
        jump_Ready = true; // wont work if true on initialise for some reason
    }

    private void Update()
    {
        // ground check raycast, half the player height and then some to make contact
        is_Grounded = Physics.Raycast(transform.position, Vector3.down, player_Height * 0.5f + 0.2f, ground_Mask);

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

    private void Player_Input()
    {
        horizontal_Input = Input.GetAxisRaw("Horizontal");
        vertical_Input = Input.GetAxisRaw("Vertical");

        // crouching
        if (Input.GetKeyUp(crouch_Key))
            is_Crouched = false;
        else if (Input.GetKey(crouch_Key))
            Crouching();
            
            
            /*
            Vector3 v3_Crouch_Height = new Vector3(body_GFX.transform.localScale.x, crouch_Height, body_GFX.transform.localScale.z);
            if (body_GFX.transform.localScale.y > crouch_Height)
                body_GFX.transform.localScale.y = Vector3.Lerp(player_Height, crouch_Height, crouch_Transition_Time);
        */
            /*
            if (Input.GetKey(jump_Key))
            {
                // reset Y velocity
                rigid_Body.angularVelocity = new Vector3(rigid_Body.angularVelocity.x, 0f, rigid_Body.angularVelocity.z);

                rigid_Body.AddForce(transform.up * jump_Force, ForceMode.Impulse); // ForceMode.Impulse to only apply once
            }
            */
        

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

    private void Crouching()
    {
        is_Crouched = true;

        if (Input.GetKey(jump_Key))
        {
;

            rigid_Body.AddForce(camera_Direction.transform.forward * (jump_Force / 2), ForceMode.Impulse); // ForceMode.Impulse to only apply once
        }

    }// end Crouching()



}// end script
