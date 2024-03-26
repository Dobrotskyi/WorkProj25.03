using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Slots
{
    public class SlotGame : MonoBehaviour
    {
        private const float SPINNING_DELAY = 0.2f;

        public static event Action SpinStarted;
        public static event Action SpinEnded;

        [SerializeField] private List<Column> _columns = new();
        [SerializeField] private Button _spinButton;
        private CombinationsFinder _finder;

        public bool InRound { private set; get; }

        public void StartGame()
        {
            if (InRound)
                return;
            SpinStarted?.Invoke();
            InRound = true;
            _spinButton.interactable = false;
            StartCoroutine(StartSpinning());
        }

        private IEnumerator StartSpinning()
        {
            foreach (Column column in _columns)
            {
                column.StartSpinning();
                yield return new WaitForSecondsRealtime(SPINNING_DELAY);
            }
        }

        private void Awake()
        {
            _finder = new();
        }

        private void OnEnable()
        {
            _columns[_columns.Count - 1].Stoped += OnLastColumnStoped;
        }

        private void OnDisable()
        {
            _columns[_columns.Count - 1].Stoped -= OnLastColumnStoped;
        }

        private void OnLastColumnStoped()
        {
            HashSet<Slot> matches = _finder.FindIn(_columns);
            float multipliers = 0;
            foreach (Slot item in matches)
            {
                item.ShowMultiplier();
                multipliers += item.Multiplier;
            }
            _spinButton.interactable = true;
            InRound = false;
        }
    }
}