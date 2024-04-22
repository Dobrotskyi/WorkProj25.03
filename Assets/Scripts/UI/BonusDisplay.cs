using Code.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class BonusDisplay : MonoBehaviour
    {
        public enum Type
        {
            Ghost,
            Shield,
            Reduce
        }
        [SerializeField] private Type _type;
        [SerializeField] private PopUp _notEnoughMoney;
        [SerializeField] private TextMeshProUGUI _amountField;
        [SerializeField] private TextMeshProUGUI _priceField;
        [SerializeField] private bool _checkIfCanUse;
        private BonusItem _item;
        private Button _button;

        public void Buy()
        {
            if (_item.Price > PlayerCurrency.Amount)
            {
                _notEnoughMoney.gameObject.SetActive(true);
                return;
            }

            _item.Buy();
        }

        private void Awake()
        {
            if (_type == Type.Ghost)
                _item = new Ghost();

            if (_type == Type.Shield)
                _item = new Shield();

            if (_type == Type.Reduce)
                _item = new LevelReducer(null);
            _button = GetComponentInChildren<Button>();
        }

        private void FixedUpdate()
        {
            if (_checkIfCanUse)
            {
                if (!_item.CanUse())
                    _button.interactable = false;
                else
                    _button.interactable = true;
            }

            if (_amountField)
                _amountField.text = _item.Amount.ToString();

            if (_priceField)
                _priceField.text = _item.Price.ToString();
        }
    }
}