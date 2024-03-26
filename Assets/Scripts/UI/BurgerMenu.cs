using System.Collections;
using UnityEngine;

namespace Code.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class BurgerMenu : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private RectTransform _body;
        private Coroutine _corutine;
        private float _progress = 1;
        private readonly Vector3 _closedScale = new(1, 0, 1);

        private bool _opened;

        private float Progress
        {
            get => _progress;
            set
            {
                if (value > 1)
                    value = 1;
                else if (value < 0)
                    value = 0;
                _progress = value;
            }
        }

        public void Toggle()
        {
            if (_corutine != null)
            {
                StopCoroutine(_corutine);
                _corutine = null;
            }
            if (_opened)
                _corutine = StartCoroutine(ChangeScale(Vector3.one, _closedScale));
            else
                _corutine = StartCoroutine(ChangeScale(_closedScale, Vector3.one));
        }

        private IEnumerator ChangeScale(Vector3 from, Vector3 to)
        {
            if (Progress != 1 || Progress != 0)
                Progress = 1 - Progress;
            else
                Progress = 0;
            _opened = !_opened;
            float t = 0;
            while (t < _duration)
            {
                t += Time.deltaTime / 2;
                Progress = t / _duration;
                Vector3 newScale = Vector3.Lerp(from,
                                                to,
                                                _curve.Evaluate(_progress));
                _body.localScale = newScale;
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime / 2;
            }
            _body.localScale = to;
        }

        private void Awake()
        {
            _body.localScale = _closedScale;
        }
    }
}