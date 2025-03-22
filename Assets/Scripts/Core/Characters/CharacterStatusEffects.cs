using Core.StatusEffects;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Characters
{
    public class CharacterStatusEffects : MonoBehaviour, IEffectible
    {
        [SerializeField]
        private CharacterStatusEffectHandler[] _handlers;

        private readonly List<EffectInstance> _currentEffects = new List<EffectInstance>();

        public IReadOnlyList<EffectInstance> CurrentEffects => _currentEffects;

        public void AddEffect(StatusEffect effect, float duration)
        {
            if (GetEffectIndex(effect, out int index))
            {
                _currentEffects[index] = new EffectInstance(effect, duration);
                return;
            }

            _currentEffects.Add(new EffectInstance(effect, duration));
            CallHandlersOnAdd(effect);
        }

        public void AddPermanentEffect(StatusEffect effect)
        {
            if (GetEffectIndex(effect, out int index))
            {
                _currentEffects[index] = new EffectInstance(effect);
                return;
            }

            _currentEffects.Add(new EffectInstance(effect));
            CallHandlersOnAdd(effect);
        }

        public void RemoveEffect(StatusEffect effect)
        {
            if (GetEffectIndex(effect, out int index) == false)
                return;

            _currentEffects.RemoveAt(index);
            CallHandlersOnRemove(effect);
        }

        private void Update()
        {
            for (int i = _currentEffects.Count - 1; i >= 0; i--)
            {
                EffectInstance effect = _currentEffects[i];

                if (effect.Actual)
                {
                    CallHandlersOnUpdate(effect.Effect);
                    continue;
                }

                _currentEffects.RemoveAt(i);
                CallHandlersOnRemove(effect.Effect);
            }
        }

        private void CallHandlersOnAdd(StatusEffect effect)
        {
            for (int i = 0; i < _handlers.Length; i++)
            {
                CharacterStatusEffectHandler handler = _handlers[i];
                if (handler.CanHandle(effect))
                    handler.HandleAdd(effect);
            }
        }

        private void CallHandlersOnUpdate(StatusEffect effect)
        {
            for (int i = 0; i < _handlers.Length; i++)
            {
                CharacterStatusEffectHandler handler = _handlers[i];
                if (handler.CanHandle(effect))
                    handler.HandleUpdate(effect);
            }
        }

        private void CallHandlersOnRemove(StatusEffect effect)
        {
            for (int i = 0; i < _handlers.Length; i++)
            {
                CharacterStatusEffectHandler handler = _handlers[i];
                if (handler.CanHandle(effect))
                    handler.HandleRemove(effect);
            }
        }

        private bool GetEffectIndex(StatusEffect effect, out int index)
        {
            for (int i = 0; i < _currentEffects.Count; i++)
            {
                if (_currentEffects[i].Effect.GetType() == effect.GetType())
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }
    }
}