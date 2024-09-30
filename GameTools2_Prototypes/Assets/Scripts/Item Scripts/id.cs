using UnityEngine;

public class id : MonoBehaviour, IInteractable
{
    public Inventory inventory;
    
    public void Interact()
    {
        print("id collected");
        inventory.has_ID = true;
        gameObject.SetActive(false);
    }
}
