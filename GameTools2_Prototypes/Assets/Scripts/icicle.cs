using System;
using UnityEngine;
using UnityEngine.AI;


interface IIcicle
{
    void Is_Frozen();
}
public class icicle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter2D called. other's tag was {other.tag}.");
        if (other.CompareTag("Enemy"))
        {
            print("slowing enemy");
            IIcicle enemy = other.GetComponent<IIcicle>();
            enemy.Is_Frozen();
        }

        Destroy(gameObject);
    }
}
