using UnityEngine;

namespace Code.Game
{
    public class OnScreenDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _renderer;
        [SerializeField] private float _minDistance = 0.1f;
        [SerializeField] private float _sensetivity = 0.5f;
        private Vector2 _previousPosition;
        private Vector2 _startPosition = Vector2.zero;

        private void OnEnable()
        {
            _previousPosition = _renderer.GetPosition(0);
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                    _startPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                Draw();
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