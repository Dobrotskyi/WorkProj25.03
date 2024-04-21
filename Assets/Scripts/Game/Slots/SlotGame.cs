using Code.UI;
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
        [SerializeField] private Bet _bet;
        [SerializeField] private WinningPopUp _winningPopUp;
        [SerializeField] private GameObject _minigameLayout;
        private CombinationsFinder _finder;
        private float _additionalMultipliers = 0;

        private LevelReducer _reducer;
        private Shield _shield;

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

        public void LaunchMiniGame()
        {
            int level = GetLevel() - 1;
            _minigameInfo[level].Layout.SetActive(true);
            _minigameLayout.SetActive(true);
        }


        public int GetLevel()
        {
            for (int i = 0; i < _minigameInfo.Count; i++)
                if (AdditionalMultipliers < _minigameInfo[i].Multipliers)
                {
                    if (i == 0)
                        return 0;
                    if (i == 1)
                        return _minigameInfo[i - 1].Level;
                    else
                        return _minigameInfo[i - 1].Level - _reducer.Reduce;
                }

            return _minigameInfo[_minigameInfo.Count - 1].Level - _reducer.Reduce;
        }

        public void StartGame()
        {
            if (InRound)
                return;
            SpinStarted?.Invoke();
            InRound = true;
            _spinButton.interactable = false;
            _bet.DisableButtons();
            StartCoroutine(StartSpinning());
            PlayerCurrency.Withdraw(_bet.Value);
        }

        public void ReduceLevel()
        {
            Debug.Log("Reduce Level");
            if (!_minigameLayout.activeSelf)
                return;

            _minigameInfo.Select(i => i.Layout).
                          Where(l => l.activeSelf).ToList().
                          ForEach(l => l.SetActive(false));
            if (_reducer.CanUse())
            {
                _reducer.TryUse();
                LaunchMiniGame();
            }

            UpdateMiniGameSlider();
        }

        public void UseShield()
        {
            if (!_minigameLayout.activeSelf)
                return;
            if (_shield.CanUse())
                _shield.TryUse();
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
            _reducer = new(this);
            _shield = new();
            _finder = new();
            _miniGameSlider.Init(_minigameInfo[0].Multipliers);
        }

        private void OnEnable()
        {
            _columns[_columns.Count - 1].Stoped += OnLastColumnStoped;
            MiniGameLevel.LevelFinished += OnLevelFinished;
        }

        private void OnDisable()
        {
            _columns[_columns.Count - 1].Stoped -= OnLastColumnStoped;
            MiniGameLevel.LevelFinished -= OnLevelFinished;
        }

        private void OnLevelFinished(bool finished)
        {
            Debug.Log(finished);
            if (Shield.IsActive && finished == false)
            {
                var current = _minigameInfo.Select(i => i.Layout).First(l => l.activeSelf);
                current.gameObject.SetActive(false);
                current.gameObject.SetActive(true);
                Shield.Reset();
                return;
            }
            Shield.Reset();
            _minigameLayout.SetActive(false);
            AdditionalMultipliers = 0;
            _reducer.Reset();
            UpdateMiniGameSlider();
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
            foreach (Slot item in mainAndSecondMatches.Item2)
            {
                item.ShowSecondMultiplier();
                AdditionalMultipliers += item.Multiplier;
            }

            if (multipliers > 0)
            {
                int winnings = (int)(multipliers * _bet.Value);
                if (PlayerCurrency.Amount + winnings < _bet.MinBet)
                {
                    int diff = _bet.MinBet - (PlayerCurrency.Amount + winnings);
                    winnings += (int)(UnityEngine.Random.Range(1, 1.3f) * diff);
                }
                _winningPopUp.InitializeWinning(winnings);
                _winningPopUp.gameObject.SetActive(true);
                PlayerCurrency.Add(winnings);
            }

            if (PlayerCurrency.Amount < _bet.MinBet)
            {
                int winnings = (int)(_bet.MinBet * UnityEngine.Random.Range(1, 1.3f));
                _winningPopUp.InitializeWinning(winnings);
                _winningPopUp.gameObject.SetActive(true);
                PlayerCurrency.Add(winnings);
            }

            UpdateMiniGameSlider();
            _spinButton.interactable = true;
            _bet.EnableButtons();
            InRound = false;
        }

        private void UpdateMiniGameSlider()
        {
            Debug.Log(GetLevel());
            int level = GetLevel();

            if (AdditionalMultipliers < _minigameInfo[0].Multipliers)
                _miniGameSlider.SetLevel(0);
            else
                _miniGameSlider.SetLevel(level);
            if (level > _minigameInfo.Count - 1)
                level = _minigameInfo.Count - 1;

            _miniGameSlider.SetValue(AdditionalMultipliers, _minigameInfo[level].Multipliers);
        }

#if UNITY_EDITOR
        public bool TestFinder;
        public float AddToAdditional = -1f;

        private void Update()
        {
            if (TestFinder)
            {
                TestFinder = false;
                _finder.FindInColumns(_columns);
            }

            if (AddToAdditional != -1f)
            {
                AdditionalMultipliers += AddToAdditional;
                UpdateMiniGameSlider();
                AddToAdditional = -1f;
            }
        }
#endif
    }
}