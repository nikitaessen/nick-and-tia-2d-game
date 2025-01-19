namespace ScriptableObject
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public ItemEffect effect;
    }
}