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

        public UnityEvent onClick => _button.onClick;

        public bool Status => _iconOnGO.activeSelf;

        public void SetStatus(bool status)
        {
            Debug.Log(status);
            _iconOnGO.SetActive(status);
            _iconOffGO.SetActive(!status);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
            if (_iconOnGO.activeSelf && _iconOffGO.activeSelf)
                SetStatus(_defaultStatus);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick() => SetStatus(!Status);
    }
}