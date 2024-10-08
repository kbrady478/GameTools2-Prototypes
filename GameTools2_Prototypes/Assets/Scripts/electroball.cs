using UnityEngine;

interface IElectroball
{
    void Is_Electrocuted();
}

public class electroball : MonoBehaviour
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
            print("electrifying enemy");
            IElectroball enemy = other.GetComponent<IElectroball>();
            enemy.Is_Electrocuted();
        }
        else if(other.CompareTag("Target"))
            reset_Enemy_Script.Reset();

        Destroy(gameObject);
    }
}
