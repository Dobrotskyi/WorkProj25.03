using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Slots
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private SlotSO _data;

        [SerializeField] private TextMeshProUGUI _multiplierField;
        [SerializeField] private TextMeshProUGUI _secondMultiplierField;
        public float Multiplier => _data.Multiplier;
        public SlotSO Data => _data;

        public void Init(SlotSO data)
        {
            _data = data;
            _image.sprite = _data.Icon;
        }

        public void ChangeTypeToRandom(SlotsCatalog catalog)
        {
            Init(catalog.GetRandomData());
        }

        public void ShowMultiplier()
        {
            if (_data.Multiplier == 0)
                return;
            _multiplierField.text = _data.Multiplier.ToString();
            _multiplierField.gameObject.SetActive(true);
        }

        public void ShowSecondMultiplier()
        {
            if (_data.Multiplier == 0)
                return;
            _secondMultiplierField.text = _data.Multiplier.ToString();
            _secondMultiplierField.gameObject.SetActive(true);
        }

        private void OnNewRound()
        {
            _multiplierField.gameObject.SetActive(false);
            _secondMultiplierField.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            SlotGame.SpinStarted += OnNewRound;
        }

        private void OnDisable()
        {
            SlotGame.SpinStarted -= OnNewRound;
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            if (_data != null)
                Init(_data);
        }

        private void OnValidate()
        {
            if (_data != null)
                Init(_data);
        }
    }
}