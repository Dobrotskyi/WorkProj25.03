using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Slots
{
    public class Slot : MonoBehaviour
    {
        private Image _image;
        private SlotSO _data;
        public float Multiplier => _data.Multiplier;
        public SlotSO Data => _data;

        public void Init(SlotSO data)
        {
            _data = data;
            _image.sprite = _data.Icon;
        }

        public void ChangeTypeToRandom()
        {

        }

        public void ShowMultiplier()
        {

        }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }
    }
}