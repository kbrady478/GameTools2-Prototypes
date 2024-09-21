using UnityEngine;

public class cam_Control : MonoBehaviour
{

    public Transform cam_Target;

    public float pos_Lerp = .02f;
    public float rot_Lerp = .01f;
    
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, cam_Target.position, pos_Lerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, cam_Target.rotation, rot_Lerp);

    }
}
