using UnityEngine;

public class deathbox : MonoBehaviour
{
    public GameObject player;
    public GameObject respawn;
    public GameObject box;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerEnter(Collider other)
    {
        print("respawn");
        if (other.CompareTag("Player"))

        {
            print("detected");
            player.transform.position = respawn.transform.position;
        }
    }
}
