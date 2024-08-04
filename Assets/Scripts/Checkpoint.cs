using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private float size;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size, size, 0));
    }
}
