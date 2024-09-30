using UnityEngine;

public class rubble : MonoBehaviour, IInteractable
{
    public Inventory inventory;
    public void Interact()
    {
        if (inventory.has_Shovel)
        {
            print("dug rubble");
            gameObject.SetActive(false);
        }
    }
}
