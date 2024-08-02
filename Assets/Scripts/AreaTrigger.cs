using System;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (Camera.main != null)
        {
            Camera.main.transform.position = targetTransform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        var boxCollider2D = GetComponent<BoxCollider2D>();
        var colliderOffset = boxCollider2D.offset;
        Gizmos.DrawWireCube(transform.position +  new Vector3(colliderOffset.x, colliderOffset.y, 0), boxCollider2D.size);
    }
}