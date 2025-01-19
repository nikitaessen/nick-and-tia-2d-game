using UnityEngine;

public abstract class ItemEffect : UnityEngine.ScriptableObject
{
    public abstract void ApplyEffect(GameObject player);
}