using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI
{
    public class OnUIPointerDown : EventTrigger
    {
        private Button _button;
        public static event Action SelectablePoiterDown;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!_button.interactable)
                return;
            SelectablePoiterDown?.Invoke();
        }

        private void OnEnable()
        {
            _button = GetComponent<Button>();
        }
    }
}