using Code.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Slots
{
    [CreateAssetMenu(menuName = "ScriptableObject/Catalog", fileName = "Catalog")]
    public class SlotsCatalog : ScriptableObject
    {
        [Header("Sum of all values must be 1")]
        [SerializeField] private List<UnitySerializableKeyValue<SlotSO, float>> _slotsAndProbabilities = new();

        public SlotSO GetRandomData()
        {
            float value = Random.Range(0, 1f);
            float chancesSum = 0;
            for (int i = 0; i < _slotsAndProbabilities.Count; i++)
            {
                chancesSum += _slotsAndProbabilities[i].Value;
                if (value <= chancesSum)
                    return _slotsAndProbabilities[i].Key;
            }

            return _slotsAndProbabilities[0].Key;
        }
    }
}