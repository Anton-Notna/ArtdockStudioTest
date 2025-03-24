using Core.StatusEffects;
using UnityEngine;

namespace Core.Abilities
{
    public class AddStatusEffect : FindComponent<IEffectible>
    {
        [SerializeField]
        private StatusEffect _effect;
        [SerializeField]
        private bool _permanent;
        [SerializeField]
        private float _statusDuration;

        protected override void ExecuteOnUnityComponent(IReadOnlyContext context, IEffectible effectible)
        {
            if (_permanent)
                effectible.AddPermanentEffect(_effect);
            else
                effectible.AddEffect(_effect, _statusDuration);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_statusDuration < 0f)
                _statusDuration = 0f;
        }
#endif
    }
}