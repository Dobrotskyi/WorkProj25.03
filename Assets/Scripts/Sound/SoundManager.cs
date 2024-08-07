using Code.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Code.Sound
{
    public class SoundManager : MonoBehaviour
    {
        private const float MUTED = -80;
        private const float UNMUTED = 0;

        public static event Action SettingChanged;

        public static SoundManager Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = Instantiate(
                        Resources.Load<SoundManager>("SoundManager"),
                        Vector3.zero,
                        Quaternion.identity
                        );
                return s_instance;
            }
        }
        private static SoundManager s_instance;

        private readonly Dictionary<SoundSettingType, string> _mixerKeys =
                         new() { { SoundSettingType.Music, "MusicVolume" },
                                 { SoundSettingType.SFX, "SFXVolume" } };

        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioSource _musicAS;
        [SerializeField] private AudioSource _clickAS;

        public bool Unmuted(SoundSettingType type)
        {
            _mixer.GetFloat(_mixerKeys[type], out float value);
            return value == UNMUTED;
        }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                SoundSettingsButton.SoundSettingsToggled += OnSoundSettingToggled;
                OnUIPointerDown.SelectablePoiterDown += PlayClickSound;
            }
            else
            {
                Debug.LogWarning($"There was more than 1 SoundManager on the scene, destroyed {gameObject.name}");
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (this != Instance)
                return;

            SoundSettingsButton.SoundSettingsToggled -= OnSoundSettingToggled;
            OnUIPointerDown.SelectablePoiterDown -= PlayClickSound;
        }

        private void OnSoundSettingToggled(SoundSettingType type)
        {
            float newValue = Unmuted(type) ? MUTED : UNMUTED;
            _mixer.SetFloat(_mixerKeys[type], newValue);
            SettingChanged?.Invoke();
        }

        private void PlayClickSound()
        {
            _clickAS.Play();
        }
    }
}