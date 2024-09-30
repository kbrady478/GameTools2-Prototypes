using UnityEngine;

public class shovel : MonoBehaviour, IInteractable
{
    public Inventory inventory;
    public void Interact()
    {
        print("shovel collected");
        inventory.has_Shovel = true;
        gameObject.SetActive(false);
    }
}
