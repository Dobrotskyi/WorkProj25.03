using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Slots
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private SlotSO _data;
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
            Debug.Log(_data.Multiplier);
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