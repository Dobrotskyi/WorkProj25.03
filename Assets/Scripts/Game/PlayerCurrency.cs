using System;
using TMPro;
using UnityEngine;

namespace Code.Game
{
    public class PlayerCurrency : MonoBehaviour
    {
        private const string KEY = "Coins";
        public static event Action Changed;

        [SerializeField] private TextMeshProUGUI _amountField;

        public static int Amount
        {
            get => PlayerPrefs.GetInt(KEY, 100);

            private set
            {
                if (value < 0)
                    value = 0;
                PlayerPrefs.SetInt(KEY, value);
                Changed?.Invoke();
            }
        }

        public static void Add(int coins)
        {
            Amount += coins;
        }

        public static bool Withdraw(int amount)
        {
            if (amount < 0)
                return false;
            if (amount > Amount)
                return false;

            Amount -= amount;
            return true;
        }

        private void View() => _amountField.text = Amount.ToString();

        private void OnEnable()
        {
            View();
            Changed += View;
        }

        private void OnDisable()
        {
            Changed -= View;
        }
    }
}