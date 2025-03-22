using Core.StatusEffects;
using System;
using UnityEngine;

namespace Core.Abilities
{
    public class AddStatusEffect : AbilityComponent
    {
        [Flags]
        private enum Target : byte
        {
            Caster = 1 << 0,
            Targets = 1 << 1,
        }

        [SerializeField]
        private StatusEffect _effect;
        [SerializeField]
        private Target _target;
        [SerializeField]
        private bool _permanent;
        [SerializeField]
        private float _statusDuration;

        protected override float StartExecute(IReadOnlyContext context)
        {
            if ((_target & Target.Caster) != 0)
                Add(context.Caster);

            if ((_target & Target.Targets) != 0)
            {
                for (int i = 0; i < context.Targets.Count; i++)
                    Add(context.Targets[i]);
            }

            return default;
        }

        private void OnValidate()
        {
            if (_statusDuration < 0f)
                _statusDuration = 0f;
        }
    
        private void Add(GameObject target)
        {
            if (target == null)
                return;

            if (target.TryGetComponent(out IEffectible effectible) == false)
                return;

            if (_permanent)
                effectible.AddPermanentEffect(_effect);
            else
                effectible.AddEffect(_effect, _statusDuration);
        }
    }
}