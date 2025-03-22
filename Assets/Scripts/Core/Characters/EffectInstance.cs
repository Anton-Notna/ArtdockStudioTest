using Core.StatusEffects;
using UnityEngine;

namespace Core.Characters
{
    public struct EffectInstance
    {
        private bool _endTimeExists;
        private float _endTime;

        public EffectInstance(StatusEffect effect)
        {
            Effect = effect;
            _endTimeExists = false;
            _endTime = default;
        }

        public EffectInstance(StatusEffect effect, float duration)
        {
            Effect = effect;
            _endTimeExists = true;
            _endTime = duration + Time.time;
        }

        public StatusEffect Effect { get; private set; }

        public float? RemainingTime => _endTimeExists ? Time.time - _endTime : null;

        public bool Actual => _endTimeExists ? Time.time < _endTime : true;
    }
}