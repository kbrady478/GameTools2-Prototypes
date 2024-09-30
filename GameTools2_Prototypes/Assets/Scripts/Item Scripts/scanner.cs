using UnityEngine;
using UnityEngine.EventSystems;

public class scanner : MonoBehaviour, IInteractable
{
   public Inventory inventory;
   public void Interact()
   {
      if (inventory.has_ID)
      {
         print("door opened");
         // open door
      }
   }
}
