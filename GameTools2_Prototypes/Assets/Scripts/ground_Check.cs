using UnityEngine;

public class ground_Check : MonoBehaviour
{
    public player_Movement player_Controller;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;

        player_Controller.Set_Grounded(true);
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            return;

        player_Controller.Set_Grounded(false);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            return;

        player_Controller.Set_Grounded(true);

    }
}
