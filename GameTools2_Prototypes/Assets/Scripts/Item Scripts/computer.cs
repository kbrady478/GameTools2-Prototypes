using UnityEngine;

public class computer : MonoBehaviour, IInteractable
{
    public switch_Map map_Swap_Script;

    public void Interact()
    {
        print("terminal used");
        map_Swap_Script.Swap_Map();
    }

}
