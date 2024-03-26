using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI
{
    [RequireComponent(typeof(Button))]
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField] private GameObject _iconOnGO;
        [SerializeField] private GameObject _iconOffGO;
        [SerializeField] private bool _defaultStatus = true;
        [SerializeField] private Button _button;

        public bool AutomaticToggle { get; private set; } = true;
        public UnityEvent onClick => _button.onClick;

        public bool Status => _iconOnGO.activeSelf;

        public void SetStatus(bool status)
        {
            _iconOnGO.SetActive(status);
            _iconOffGO.SetActive(!status);
        }

        private void OnEnable()
        {
            if (_iconOnGO.activeSelf && _iconOffGO.activeSelf)
                SetStatus(_defaultStatus);
        }
    }
}