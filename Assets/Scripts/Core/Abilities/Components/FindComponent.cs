using System;
using UnityEngine;

namespace Core.Abilities
{
    public abstract class FindComponent<TUnityComponent> : AbilityComponent
    {
        [SerializeField]
        private GameObjectTarget _target;

        protected override float StartExecute(IReadOnlyContext context)
        {
            if ((_target & GameObjectTarget.Caster) != 0)
                Find(context, context.Caster);

            if ((_target & GameObjectTarget.Targets) != 0)
            {
                for (int i = 0; i < context.Targets.Count; i++)
                    Find(context, context.Targets[i]);
            }

            return default;
        }

        protected abstract void ExecuteOnUnityComponent(IReadOnlyContext context, TUnityComponent component);

        private void Find(IReadOnlyContext context, GameObject gameObject)
        {
            if (gameObject == null)
                return;

            if (gameObject.TryGetComponent(out TUnityComponent component))
                ExecuteOnUnityComponent(context, component);
        }
    }
}