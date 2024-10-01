using UnityEngine;
using UnityEngine.EventSystems;

public class scanner : MonoBehaviour, IInteractable
{
   public Inventory inventory;
   public GameObject door;
   public void Interact()
   {
      if (inventory.has_ID)
      {
         print("door opened");
         door.SetActive(false);
         // open door
      }
   }
}
