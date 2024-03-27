using UnityEngine;

namespace Code.Game.Slots
{
    [CreateAssetMenu(menuName = "ScriptableObject/Slot", fileName = "Slot")]
    public class SlotSO : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _multiplier;

        public float Multiplier => _multiplier;
        public Sprite Icon => _icon;
    }
}