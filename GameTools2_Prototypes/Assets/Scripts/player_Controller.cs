// tutorial used: https://www.youtube.com/watch?v=4HpC--2iowE
using System;
using UnityEngine;

public class player_Controller : MonoBehaviour
{
    
    public CharacterController controller;
    public Transform camera;

    public float speed = 6f;
    public float turn_Smoothening = 0.1f;
    public float turn_Smooth_Velocity;

    // Jumping
    public Transform ground_Check;
    public LayerMask ground_Mask;
    public float ground_Distance = 0.4f; // for groundcheck sphere
    public float gravity = -2f;
    public float jump_Height = 3f;

    private Vector3 velocity; // for gravity
    private bool is_Grounded;


    // Update is called once per frame
    void Update()
    {
        // Direction check
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Ground Check
        // Creates invisible sphere at object, detects collisions with ground_Mask layer
        is_Grounded = Physics.CheckSphere(ground_Check.position, ground_Distance, ground_Mask);

        if (is_Grounded && velocity.y < 0)
        {
            // Not 0 because sphere protrudes  down, would cause floating effect
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && is_Grounded)
        {
            velocity.y = MathF.Sqrt(jump_Height * -2f * gravity);
        }

            // Directional movement
        if (direction.magnitude >= 0.1f)
        {
            float target_Angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_Angle, ref turn_Smooth_Velocity, turn_Smoothening);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move_Direction = Quaternion.Euler(0f, target_Angle, 0f) * Vector3.forward;
            controller.Move(move_Direction.normalized * speed * Time.deltaTime);

        }// end Directional movement

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }// end Update()l

}// end player_Controller
