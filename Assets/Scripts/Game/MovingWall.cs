using System.Collections;
using UnityEngine;

namespace Code.Game
{
    public class MovingWall : MonoBehaviour
    {
        [SerializeField] private Transform _point1;
        [SerializeField] private Transform _point2;
        [SerializeField] private Transform _wall;
        [SerializeField] private float _speed = 0.5f;
        private Transform _currentEndPoint;

        private void OnEnable()
        {
            _wall.position = _point1.position;
            _currentEndPoint = _point2;
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            while (true)
            {
                _wall.Translate((_currentEndPoint.position - _wall.position).normalized * _speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
                if (Vector2.Distance(_wall.position, _currentEndPoint.position) < 0.1f)
                {
                    if (_currentEndPoint == _point1)
                        _currentEndPoint = _point2;
                    else
                        _currentEndPoint = _point1;
                }
            }
        }
    }
}