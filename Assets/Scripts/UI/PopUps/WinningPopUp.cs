using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class WinningPopUp : PopUp
    {
        [SerializeField] private TextMeshProUGUI _coinsAmountField;

        public void InitializeWinning(int amount)
        {
            Initialize("congratulations", "You won:");
            _coinsAmountField.text = "+" + amount.ToString();
        }
    }
}