using System;
using UnityEngine;

public class switch_Map : MonoBehaviour, IInteractable
{
    public GameObject map1;
    public GameObject map2;

    public bool swap_Map = false;
    //public bool is_Map1 = true;
    //public bool is_Map2 = false;
    
    public void Interact()
    {
        print("switching map");
        swap_Map = !swap_Map;   
        
        if (swap_Map == true)
        {
            map1.SetActive(false);
            map2.SetActive(true);
        }
        else if (swap_Map == false) //&& map1.activeInHierarchy == false)
        {
            map1.SetActive(true);
            map2.SetActive(false);
        }
    }
}
