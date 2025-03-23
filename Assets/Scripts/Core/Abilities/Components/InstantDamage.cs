using Core.Damages;
using UnityEngine;

namespace Core.Abilities
{
    public class InstantDamage : AbilityComponent
    {
        [SerializeField]
        private int _damage;
        [SerializeField]
        private GameObjectTarget _target;

        protected override float StartExecute(IReadOnlyContext context)
        {
            if ((_target & GameObjectTarget.Caster) != 0)
                Damage(context.Caster);

            if ((_target & GameObjectTarget.Targets) != 0)
            {
                for (int i = 0; i < context.Targets.Count; i++)
                    Damage(context.Targets[i]);
            }

            return default;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_damage < 0)
                _damage = 0;
        }
#endif

        private void Damage(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            if (gameObject.TryGetComponent(out IDamageable damageable) == false)
                return;

            damageable.TakeDamage(new Damage()
            {
                Amount = _damage,
            });
        }
    }
}