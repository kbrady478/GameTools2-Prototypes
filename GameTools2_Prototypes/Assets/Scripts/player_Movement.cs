// Tutorial used: https://www.youtube.com/watch?v=1LtePgzeqjQ


using UnityEngine;
using UnityEngine.InputSystem;

public class player_Movement : MonoBehaviour
{

    public Rigidbody rigid_Body;
    public GameObject cam_Holder;

    public float speed, sensitivity, max_Force, jump_Force;

    public bool grounded;

    private Vector2 move, look;
    
    private float look_Rotation;


    public void On_Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void On_Look(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void On_Jump(InputAction.CallbackContext context)
    {
        Jump();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void FixedUpdate()
    {
        Move();
    }// End FixedUpdate()
/*
    private void LateUpdate()
    {
         Look();
    }// End LateUpdate()


    private void Look()
    {
        // Turn 
        transform.Rotate(Vector3.up * look.x * sensitivity);

        // Look
        look_Rotation += (-look.y * sensitivity);
        cam_Holder.transform.eulerAngles = new Vector3(look_Rotation, cam_Holder.transform.eulerAngles.y, cam_Holder.transform.eulerAngles.z);

    }// End Look()
*/

    private void Move()
    {
        // Find target velocity
        Vector3 current_Velocity = rigid_Body.linearVelocity;
        Vector3 target_Velocity = new Vector3(move.x, 0, move.y);
        target_Velocity *= speed;

        // Align direction
        target_Velocity = transform.TransformDirection(target_Velocity);

        // Calculate forces
        Vector3 velocity_Change = (target_Velocity - current_Velocity);
        velocity_Change = new Vector3(velocity_Change.x, 0, velocity_Change.z);

        // Limit force
        Vector3.ClampMagnitude(velocity_Change, max_Force);

        rigid_Body.AddForce(velocity_Change, ForceMode.VelocityChange);
    }// End Move()


    private void Jump()
    {
        Vector3 jump_Forces = Vector3.zero;

        if (grounded)
        {
            jump_Forces = Vector3.up * jump_Force;
        }

        rigid_Body.AddForce(jump_Forces, ForceMode.VelocityChange);
    }// End Jump()


    public void Set_Grounded(bool state)
    {
        grounded = state;
    }// End Set_Grounded()


}// End
