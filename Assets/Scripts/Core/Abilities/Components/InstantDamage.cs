using Core.Damages;
using UnityEngine;

namespace Core.Abilities
{
    public class InstantDamage : FindComponent<IDamageable>
    {
        [SerializeField]
        private int _damage;

        public override string Description => "Damage targets.";

        protected override void ExecuteOnUnityComponent(IReadOnlyContext context, IDamageable component)
        {
            component.TakeDamage(new Damage()
            {
                Amount = _damage,
            });
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_damage < 0)
                _damage = 0;
        }
#endif
    }
}