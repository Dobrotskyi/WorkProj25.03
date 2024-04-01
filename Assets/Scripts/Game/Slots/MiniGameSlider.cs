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
        private int _level = 0;

        public void SetValue(float value, float maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = value;
        }

        public void SetLevel(int level)
        {
            _level = level;
            _levelField.text = level.ToString();
            OnValueChanged();
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

            Debug.Log(_level);
            if (_level >= 1 && !_button.interactable)
                _button.interactable = true;

            else if (_level == 0 && _button.interactable)
                _button.interactable = false;
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