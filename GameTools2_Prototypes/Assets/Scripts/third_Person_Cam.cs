using UnityEngine;

public class third_Person_Cam : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform player_Obj;
    public Rigidbody rigid_Body;

    public float rotation_Speed;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
    }


    private void Update()
    {

        // find rotate orientation
        Vector3 view_Direction = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = view_Direction.normalized;

        // rotate player obj
        float horizontal_Input = Input.GetAxis("Horizontal");
        float vertical_Input = Input.GetAxis("Vertical");
        Vector3 input_Direction = orientation.forward * vertical_Input + orientation.right * horizontal_Input;

        if (input_Direction != Vector3.zero)
            player_Obj.forward = Vector3.Slerp(player_Obj.forward, input_Direction.normalized, Time.deltaTime * rotation_Speed);


    }// end Update()

}
