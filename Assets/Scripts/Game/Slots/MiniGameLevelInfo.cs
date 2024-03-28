using System;
using UnityEngine;

namespace Code.Game.Slots
{
    [Serializable]
    public class MiniGameLevelInfo
    {
        [SerializeField] private float _multipliersToGet;
        [SerializeField] private int _level;
        [SerializeField] private GameObject _layout;

        public float Multipliers => _multipliersToGet;
        public int Level => _level;
        public GameObject Layout => _layout;
    }
}