using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private List<MiniGameLevelInfo> _minigameInfo = new();
        [SerializeField] private MiniGameSlider _miniGameSlider;
        private CombinationsFinder _finder;
        private float _additionalMultipliers = 0;

        public bool InRound { private set; get; }
        public float AdditionalMultipliers
        {
            get => _additionalMultipliers;
            set
            {
                float max = _minigameInfo[_minigameInfo.Count - 1].Multipliers;
                if (value > max)
                    value = max;
                _additionalMultipliers = value;

            }
        }

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
            _miniGameSlider.Init(_minigameInfo[0].Multipliers);
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
            (HashSet<Slot>, HashSet<Slot>) mainAndSecondMatches = _finder.FindInColumns(_columns);
            float multipliers = 0;
            foreach (Slot item in mainAndSecondMatches.Item1)
            {
                item.ShowMultiplier();
                multipliers += item.Multiplier;
            }
            AdditionalMultipliers += mainAndSecondMatches.Item2.Sum(s => s.Data.Multiplier);
            UpdateMiniGameSlider();
            _spinButton.interactable = true;
            InRound = false;
        }

        private void UpdateMiniGameSlider()
        {
            _miniGameSlider.SetValue(AdditionalMultipliers);
            for (int i = 0; i < _minigameInfo.Count; i++)
            {
                if (AdditionalMultipliers < _minigameInfo[i].Multipliers)
                {
                    _miniGameSlider.SetLevel(i);
                    return;
                }
            }
            _miniGameSlider.SetLevel(_minigameInfo.Count);
        }

#if UNITY_EDITOR
        public bool TestFinder;

        private void Update()
        {
            if (TestFinder)
            {
                TestFinder = false;
                _finder.FindInColumns(_columns);
            }
        }
#endif
    }
}