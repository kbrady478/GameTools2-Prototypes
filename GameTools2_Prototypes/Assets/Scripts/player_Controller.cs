// tutorial used: https://www.youtube.com/watch?v=4HpC--2iowE
using UnityEngine;

public class player_Controller : MonoBehaviour
{
    
    public CharacterController controller;
    public Transform camera;

    public float speed = 6f;
    public float turn_Smoothening = 0.1f;
    public float turn_Smooth_Velocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float target_Angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_Angle, ref turn_Smooth_Velocity, turn_Smoothening);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move_Direction = Quaternion.Euler(0f, target_Angle, 0f) * Vector3.forward;
            controller.Move(move_Direction.normalized * speed * Time.deltaTime);

        }

    }// end Update()l

}// end player_Controller
