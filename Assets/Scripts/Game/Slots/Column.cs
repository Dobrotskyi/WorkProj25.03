using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Slots
{
    public class Column : MonoBehaviour
    {
        private const float DURATION = 1f;

        public event Action Stoped;

        [SerializeField] private VerticalLayoutGroup _movingPart;
        [SerializeField] private AnimationCurve _spinningCurve;
        [SerializeField] private SlotsCatalog _catalog;
        [SerializeField] private AudioSource _as;
        private RectTransform _movingPartRect;

        public bool IsSpinning { private set; get; }

        public List<Slot> GetSlotsInColumn() => _movingPart.transform.GetChild(0).
                                                  GetComponentsInChildren<Slot>().ToList();

        public void StartSpinning()
        {
            if (IsSpinning)
                return;
            IsSpinning = true;
            StartCoroutine(Spinning());
        }

        private void Start()
        {
            _movingPartRect = _movingPart.GetComponent<RectTransform>();
            StartCoroutine(SpawnNextSlotPart());//!!!!!!
        }

        private IEnumerator Spinning()
        {
            yield return SpawnNextSlotPart();
            float startY = _movingPartRect.anchoredPosition.y;
            float endY = _movingPartRect.anchoredPosition.y + _movingPartRect.rect.height;
            float t = 0;
            while (t < DURATION)
            {
                float progress = t / DURATION;
                float speed = _spinningCurve.Evaluate(progress);
                Vector2 newPos = _movingPartRect.anchoredPosition;
                newPos.y = startY + speed * (endY - startY);
                _movingPartRect.anchoredPosition = newPos;
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return FinishSpin();
        }

        private IEnumerator FinishSpin()
        {
            Transform firstGroup = _movingPart.transform.GetChild(0);
            Destroy(firstGroup.gameObject);
            Vector2 newPos = _movingPartRect.anchoredPosition;
            newPos.y = 0;
            _movingPartRect.anchoredPosition = newPos;
            CallRebuilder();
            yield return new WaitForEndOfFrame();
            IsSpinning = false;
            Stoped?.Invoke();
            _as.Play();
        }

        private IEnumerator SpawnNextSlotPart()
        {
            Transform slotGroup = _movingPart.transform.GetChild(0);
            Transform newSlotGroup = Instantiate(slotGroup, _movingPart.transform);
            var newSlots = newSlotGroup.transform.GetComponentsInChildren<Slot>();
            foreach (var slot in newSlots)
                slot.ChangeTypeToRandom(_catalog);
            CallRebuilder();
            yield return new WaitForEndOfFrame();
        }

        private void CallRebuilder() => LayoutRebuilder.ForceRebuildLayoutImmediate(_movingPart.transform as RectTransform);

#if UNITY_EDITOR
        public bool testSpawn;
        public bool testSpin;
        private void Update()
        {
            if (testSpawn)
            {
                testSpawn = false;
                StartCoroutine(SpawnNextSlotPart());
            }

            if (testSpin)
            {
                testSpin = false;
                StartCoroutine(Spinning());
            }
        }
#endif
    }
}