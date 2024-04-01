using System;
using UnityEngine;

namespace Code.Game
{
    [CreateAssetMenu(menuName = "ScriptableObject/Item", fileName = "Item")]
    public class ShopItemSO : ScriptableObject
    {
        public static event Action AmountChanged;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _price;

        public int Price => _price;
        public int Amount => PlayerPrefs.GetInt(_name, 0);
        public Sprite Icon => _sprite;

        public void Add()
        {
            PlayerPrefs.SetInt(_name, Amount + 1);
            AmountChanged?.Invoke();
        }

        public bool TryUse()
        {
            if (Amount < 1)
                return false;

            PlayerPrefs.SetInt(_name, Amount - 1);
            AmountChanged?.Invoke();
            return true;
        }
    }
}