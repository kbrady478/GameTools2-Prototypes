using System;
using UnityEngine;
using UnityEngine.AI;


interface IIcicle
{
    void Is_Frozen();
}
public class icicle : MonoBehaviour
{
    private reset_Enemy reset_Enemy_Script;
    private GameObject target;
    
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        reset_Enemy_Script = target.GetComponent<reset_Enemy>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter2D called. other's tag was {other.tag}.");
        if (other.CompareTag("Enemy"))
        {
            print("slowing enemy");
            IIcicle enemy = other.GetComponent<IIcicle>();
            enemy.Is_Frozen();
        }
        else if(other.CompareTag("Target"))
            reset_Enemy_Script.Reset();

        Destroy(gameObject);
    }
}
