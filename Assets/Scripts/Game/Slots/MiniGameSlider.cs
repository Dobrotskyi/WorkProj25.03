using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Slots
{
    public class MiniGameSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Sprite _completedFill;
        [SerializeField] private Sprite _defaultFill;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _levelField;
        [SerializeField] private Button _button;

        public void SetValue(float value)
        {
            _slider.value = value;
        }

        public void SetLevel(float level)
        {
            _levelField.text = level.ToString();
        }

        public void Init(float max)
        {
            _slider.maxValue = max;
            _slider.value = 0;
        }

        public void OnValueChanged()
        {
            if (_slider.value == _slider.maxValue)
                _fillImage.sprite = _completedFill;
            else
            {
                if (_fillImage.sprite == _completedFill)
                    _fillImage.sprite = _defaultFill;
            }

            if (_slider.value == _slider.maxValue && !_button.interactable)
            {
                if (!_button.interactable)
                    _button.interactable = true;
            }
            else
            {
                if (_button.interactable)
                    _button.interactable = false;
            }
        }

        private void OnValidate()
        {
            OnValueChanged();
        }

        private void OnEnable()
        {
            _button.interactable = false;
        }
    }
}