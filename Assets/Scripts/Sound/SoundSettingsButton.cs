using Code.UI;
using System;
using UnityEngine;

namespace Code.Sound
{
    [RequireComponent(typeof(ToggleButton))]
    public class SoundSettingsButton : MonoBehaviour
    {
        public static event Action<SoundSettingType> SoundSettingsToggled;

        [SerializeField] private SoundSettingType _type;
        private ToggleButton _toggle;

        private void Awake()
        {
            _toggle = GetComponent<ToggleButton>();
        }

        private void OnEnable()
        {
            _toggle.onClick.AddListener(OnButtonClicked);
            _toggle.SetStatus(!SoundManager.Instance.Muted(_type));
        }

        private void OnDisable()
        {
            _toggle.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked() => SoundSettingsToggled?.Invoke(_type);
    }
}