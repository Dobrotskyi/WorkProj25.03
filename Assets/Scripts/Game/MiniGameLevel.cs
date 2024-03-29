using Code.Game.Slots;
using Code.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game
{
    public class MiniGameLevel : MonoBehaviour
    {
        public static event Action<bool> LevelFinished;
        [SerializeField] private RectTransform _from;
        [SerializeField] private RectTransform _to;
        [SerializeField] private OnScreenDrawer _drawer;
        [SerializeField] private float _minDistance = 1f;
        [SerializeField] private WinningPopUp _popUp;
        [SerializeField] private PopUp _losingPopUp;
        [SerializeField] private float _bet = 100;
        [SerializeField] private Rigidbody2D _collisionDummy;
        private Button _closeButton;
        private SlotGame _game;

        private void Awake()
        {
            _game = FindObjectOfType<SlotGame>();
        }

        private void OnEnable()
        {
            StartCoroutine(SetStartPosition());
            _collisionDummy.GetComponent<DummyCollisionHandler>().Entered += OnDummyTriggerEnter;
            _drawer.Blocked = false;
        }

        private void OnDisable()
        {
            _collisionDummy.GetComponent<DummyCollisionHandler>().Entered -= OnDummyTriggerEnter;
            if (_closeButton != null)
                _closeButton.onClick.RemoveListener(Close);
        }

        private IEnumerator SetStartPosition()
        {
            yield return new WaitForEndOfFrame();
            _drawer.TurnOn(_from.position);
        }

        private void Update()
        {
            if (Vector2.Distance(_to.position, _drawer.WorldSpaceCurrentPositon) < _minDistance)
            {
                int winning = (int)(_game.AdditionalMultipliers * _bet);
                _popUp.InitializeWinning(winning);
                _popUp.gameObject.SetActive(true);
                PlayerCurrency.Add(winning);
                Close();
            }
        }

        private void LateUpdate()
        {
            _collisionDummy.position = _drawer.WorldSpaceCurrentPositon;
        }

        private void OnDummyTriggerEnter()
        {
            _drawer.Blocked = true;
            _losingPopUp.LosingText();
            _losingPopUp.gameObject.SetActive(true);
            _closeButton = _losingPopUp.GetComponentInChildren<Button>();
            _closeButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            _drawer.TurnOff();
            gameObject.SetActive(false);
            LevelFinished?.Invoke(false);
        }
    }
}
