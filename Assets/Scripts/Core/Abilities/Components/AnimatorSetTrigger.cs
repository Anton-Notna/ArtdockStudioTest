using Core.Gfx;
using System;
using UnityEngine;

namespace Core.Abilities
{
    public class AnimatorSetTrigger : AbilityComponent
    {
        [Flags]
        private enum Target : byte
        {
            Caster = 1 << 0,
            Targets = 1 << 1,
        }

        [SerializeField]
        private Target _target;
        [SerializeField]
        private AnimatorTrigger _trigger;

        public override string Description => "Set AnimatorTrigger on Caster's/Target's CharacterAnimator component";

        protected override float StartExecute(IReadOnlyContext context)
        {
            if ((_target & Target.Caster) != 0 && context.Caster != null && context.Caster.TryGetComponent(out CharacterAnimator caster))
                caster.SetTrigger(_trigger);

            if ((_target & Target.Targets) != 0)
            {
                for (int i = 0; i < context.Targets.Count; i++)
                {
                    GameObject targetObject = context.Targets[i];
                    if (targetObject == null)
                        continue;

                    if (targetObject.TryGetComponent(out CharacterAnimator target))
                        target.SetTrigger(_trigger);
                }
            }

            return default;
        }
    }
}