using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    [RequireComponent(typeof(Animator))]
    public class SceneLoading : MonoBehaviour
    {
        public static event Action ChangingScene;
        private static SceneLoading s_instance;
        private static bool s_openWithAnimation;

        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _percentageField;
        private Animator _animator;
        private AsyncOperation _loadingOperation;

        public static void LoadScene(string sceneName)
        {
            ChangingScene?.Invoke();
            s_instance._animator.SetTrigger("StartLoading");
            s_instance._loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            s_instance._loadingOperation.allowSceneActivation = false;
            s_instance.StartCoroutine(Loading());
        }

        private static IEnumerator Loading()
        {
            while (!s_instance._loadingOperation.isDone)
            {
                float progress = Mathf.Clamp01(s_instance._loadingOperation.progress / 0.9f);
                if (progress < 0.1)
                    progress = 0.1f;
                s_instance._slider.value = progress;
                s_instance._percentageField.text = "Loading... " + ((int)(progress * 100f)).ToString() + "%";
                yield return new WaitForEndOfFrame();
            }
        }

        public void OnAnimationEnded()
        {
            s_openWithAnimation = true;
            s_instance._loadingOperation.allowSceneActivation = true;
        }

        private void Awake()
        {
            s_instance = this;
            _animator = GetComponent<Animator>();
            if (s_openWithAnimation)
                _animator.SetTrigger("EndLoading");
        }
    }
}
