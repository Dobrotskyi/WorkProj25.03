using System;
using UnityEngine.EventSystems;

namespace Code.UI
{
    public class OnUIPointerDown : EventTrigger
    {
        public static event Action SelectablePoiterDown;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            SelectablePoiterDown?.Invoke();
        }
    }
}