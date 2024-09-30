using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class switch_Map : MonoBehaviour
{
    public GameObject map1;
    public GameObject map2;
    public GameObject text_Holder;
    public TextMeshProUGUI timer_Text;

    public float starting_Time;
    public float remaining_Time;
    public bool swap_Map = false;
    
    public void Swap_Map()
    {
        print("switching map");
        swap_Map = !swap_Map;   
       
        if (swap_Map == true)
        {
            map1.SetActive(false);
            map2.SetActive(true);
            
            text_Holder.SetActive(true);
            
            remaining_Time = starting_Time;
            StartCoroutine(Timer());
        }
        else if (swap_Map == false) //&& map1.activeInHierarchy == false)
        {
            map1.SetActive(true);
            map2.SetActive(false);
            
            text_Holder.SetActive(false);
        }
        
    }// end Swap_Map()
    
    private IEnumerator Timer()
    {
        while (remaining_Time > 0)
        {
            timer_Text.text = remaining_Time.ToString("F3");
            remaining_Time -= Time.deltaTime;
            if (remaining_Time <= 0)
            {
                remaining_Time = 0;
                Swap_Map();
            }
            yield return null;
        }
    }
}// end script
