using Code.Game.Slots;
using System;
using UnityEngine;

namespace Code.Game
{
    public abstract class BonusItem
    {
        public event Action Used;

        public abstract string Key { get; }

        public int Amount
        {
            private set
            {
                if (value < 0) value = 0;
                PlayerPrefs.SetInt(Key, value);
            }

            get => PlayerPrefs.GetInt(Key, 0);
        }

        public static int GetAmount(BonusItem item)
        {
            return PlayerPrefs.GetInt(item.Key, 0);
        }

        public bool CanUse()
        {
            //if (!AdditionalCheck() || Amount < 1)
            //    return false;
            if (!AdditionalCheck())
                return false;
            return true;
        }

        public void TryUse()
        {
            if (!CanUse())
                return;

            Use();
            PlayerPrefs.SetInt(Key, Amount - 1);
            Used?.Invoke();
        }

        protected virtual bool AdditionalCheck() => true;

        protected abstract void Use();
    }

    public class LevelReducer : BonusItem
    {
        private SlotGame _game;

        public LevelReducer(SlotGame game)
        {
            _game = game;
        }

        public override string Key => "LevelReducer";
        public int Reduce { private set; get; } = 0;

        public void Reset() => Reduce = 0;

        protected override bool AdditionalCheck()
        {
            if (_game.GetLevel() <= 1)
                return false;
            return true;
        }

        protected override void Use()
        {
            Reduce++;
        }
    }

    public class Shield : BonusItem
    {
        public override string Key => "Shield";
        public static bool IsActive { private set; get; }
        public static event Action<bool> Changed;

        public static void Reset()
        {
            IsActive = false;
            Changed?.Invoke(false);
        }

        protected override bool AdditionalCheck()
        {
            if (IsActive || Ghost.IsActive) return false;
            return true;
        }

        protected override void Use()
        {
            s_Used();
        }

        private static void s_Used()
        {
            IsActive = true;
            Changed?.Invoke(true);
        }
    }

    public class Ghost : BonusItem
    {
        public override string Key => "Ghost";
        public static bool IsActive { private set; get; }

        protected override bool AdditionalCheck()
        {
            if (IsActive || Shield.IsActive) return false;
            return true;
        }

        protected override void Use()
        {
            IsActive = true;
        }
    }
}