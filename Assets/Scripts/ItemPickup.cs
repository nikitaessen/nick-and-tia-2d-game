using ScriptableObject;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other.gameObject);
        }
    }

    private void Pickup(GameObject player)
    {
        if (itemData.effect != null)
        {
            itemData.effect.ApplyEffect(player);
        }

        Destroy(gameObject);
    }        
}