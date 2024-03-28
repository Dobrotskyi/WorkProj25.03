using Code.Game;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class Bet : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountField;
        [SerializeField] private List<Button> _buttons;
        private int _defaultStep = 100;
        private int _value = 100;

        public int Value => _value;

        public void DisableButtons()
        {
            foreach (var button in _buttons)
                button.interactable = false;
        }

        public void EnableButtons()
        {
            foreach (var button in _buttons)
                button.interactable = true;
            if (_value > PlayerCurrency.Amount)
                _value = PlayerCurrency.Amount / _defaultStep * _defaultStep;

        }

        public void Add()
        {
            if (_value + _defaultStep > PlayerCurrency.Amount)
                return;
            _value += _defaultStep;
            View();
        }

        public void Decrease()
        {
            if (_value - _defaultStep < _defaultStep) return;
            _value -= _defaultStep;
            View();
        }

        private void OnEnable()
        {
            View();
        }

        private void View()
        {
            _amountField.text = _value.ToString();
        }
    }
}