using UnityEngine;

public class LanternLight : MonoBehaviour
{
    public GameObject light;

    void Start()
    {
        light.GetComponent<Light>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            
            light.GetComponent<Light>().enabled = true;
        }
    }
}
