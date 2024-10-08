using UnityEngine;

interface IElectroball
{
    void Is_Electrocuted();
}

public class electroball : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter2D called. other's tag was {other.tag}.");
        if (other.CompareTag("Enemy"))
        {
            print("electrifying enemy");
            IElectroball enemy = other.GetComponent<IElectroball>();
            enemy.Is_Electrocuted();
        }

        Destroy(gameObject);
    }
}
