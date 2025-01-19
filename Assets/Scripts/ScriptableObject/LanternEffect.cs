using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "LanternEffect", menuName = "GameEffects/Lantern Effect")]
    public class LanternEffect : ItemEffect
    {
        public override void ApplyEffect(GameObject player)
        {
            var playerLight = player.GetComponent<PlayerLight>();
            playerLight.TurnOnLanternLight();
        }
    }
}