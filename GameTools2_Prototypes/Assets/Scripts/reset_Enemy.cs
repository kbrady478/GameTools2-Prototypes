using System;
using UnityEngine;


public class reset_Enemy : MonoBehaviour
{

    public GameObject enemy_Prefab;
    public Transform spawn_Point;
    
    private GameObject current_Enemy;

    private void Awake()
    {
        current_Enemy = Instantiate(enemy_Prefab, spawn_Point.position, Quaternion.identity);
        
    }

    public void Reset()
    {
        print("reset enemy");
        Destroy(current_Enemy);
        
        current_Enemy = Instantiate(enemy_Prefab, spawn_Point.position, Quaternion.identity);
    }

}
