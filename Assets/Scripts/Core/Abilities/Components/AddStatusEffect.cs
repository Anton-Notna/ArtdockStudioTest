using Core.StatusEffects;
using System;
using UnityEngine;

namespace Core.Abilities
{
    public class AddStatusEffect : AbilityComponent
    {
        [SerializeField]
        private StatusEffect _effect;
        [SerializeField]
        private GameObjectTarget _target;
        [SerializeField]
        private bool _permanent;
        [SerializeField]
        private float _statusDuration;

        protected override float StartExecute(IReadOnlyContext context)
        {
            if ((_target & GameObjectTarget.Caster) != 0)
                Add(context.Caster);

            if ((_target & GameObjectTarget.Targets) != 0)
            {
                for (int i = 0; i < context.Targets.Count; i++)
                    Add(context.Targets[i]);
            }

            return default;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_statusDuration < 0f)
                _statusDuration = 0f;
        }
#endif

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