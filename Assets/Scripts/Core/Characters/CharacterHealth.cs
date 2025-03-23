using Core.Damages;
using Core.StatusEffects;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    public class CharacterHealth : CharacterStatusEffectHandler<ContinuousDamageEffect>, IHealth
    {
        [SerializeField]
        private int _health = 10;
        [SerializeField]
        private UnityEvent<Damage, IHealth> _damageTaken;

        private float _lastContinuousDamage = float.MinValue;
        private bool _dead;

        public int Health => _health;

        public event Action<Damage, IHealth> DamageTaken
        {
            add => _damageTaken.AddListener(value.Invoke);
            remove => _damageTaken.RemoveListener(value.Invoke);
        }

        public bool TakeDamage(Damage damage)
        {
            if (_dead)
                return false;

            if (damage.Amount <= 0)
                return false;

            _health -= damage.Amount;
            if (_health < 0)
                _health = 0;

            _dead = _health == 0;
            _damageTaken.Invoke(damage, this);

            if (_dead)
                Destroy(gameObject);

            return true;
        }

        protected override void HandleUpdate(ContinuousDamageEffect effect)
        {
            if (_lastContinuousDamage + effect.CycleDelay < Time.time)
                return;

            _lastContinuousDamage = Time.time;
            TakeDamage(new Damage()
            {
                Amount = effect.DamagePerCycle,
            });
        }

        private void OnValidate()
        {
            if (_health < 0)
                _health = 0;
        }
    }
}