using UnityEngine;
using UnityEngine.UI;

namespace Code.Game
{
    public class OnScreenDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _renderer;
        [SerializeField] private float _minDistance = 0.1f;
        [SerializeField] private float _sensetivity = 0.5f;
        [SerializeField] private Transform _fromPosition;
        [SerializeField] private Image _shieldIcon;
        [SerializeField] private Image _ghostIcon;
        [SerializeField] private Image _defaultFace;

        private Vector2 _previousPosition;
        private Vector2 _startPosition = Vector2.zero;
        private bool _isOn = true;

        public bool Blocked;

        public Vector2 WorldSpaceCurrentPositon => _renderer.GetPosition(_renderer.positionCount - 1);

        public void TurnOn(Vector2 startPosition)
        {
            _isOn = true;
            _renderer.positionCount = 1;
            _renderer.SetPosition(0, startPosition);
            _previousPosition = _renderer.GetPosition(0);
        }

        public void TurnOff()
        {
            _renderer.positionCount = 1;
            _isOn = false;
        }

        private void Update()
        {
            if (Input.touchCount > 0 && _isOn && !Blocked)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                    _startPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                Draw();
            }
        }

        private void LateUpdate()
        {
            Vector3 lastPos = _renderer.GetPosition(_renderer.positionCount - 1);
            _shieldIcon.transform.position = lastPos;
            _ghostIcon.transform.position = lastPos;
            _defaultFace.transform.position = lastPos;
            if (Shield.IsActive)
            {
                if (!_shieldIcon.gameObject.activeSelf)
                    _shieldIcon.gameObject.SetActive(true);
            }
            else
            {
                if (_shieldIcon.gameObject.activeSelf)
                    _shieldIcon.gameObject.SetActive(false);
            }

            if (Ghost.IsActive)
            {
                if (!_ghostIcon.gameObject.activeSelf)
                    _ghostIcon.gameObject.SetActive(true);
            }
            else
            {
                if (_ghostIcon.gameObject.activeSelf)
                    _ghostIcon.gameObject.SetActive(false);
            }

            if (!Ghost.IsActive && !Shield.IsActive)
            {
                if (!_defaultFace.gameObject.activeSelf)
                    _defaultFace.gameObject.SetActive(true);
            }
            else
            {
                if (_defaultFace.gameObject.activeSelf)
                    _defaultFace.gameObject.SetActive(false);
            }
        }

        private void Draw()
        {
            Vector2 currentPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            if (Vector2.Distance(currentPosition, _startPosition) < _minDistance)
                return;

            Vector2 diff = currentPosition - _startPosition;
            diff *= _sensetivity;
            _renderer.positionCount++;
            _renderer.SetPosition(_renderer.positionCount - 1, _previousPosition + diff);
            _previousPosition += diff;
            _startPosition = currentPosition;
        }
    }
}