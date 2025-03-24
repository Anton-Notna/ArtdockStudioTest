using Core.Gfx;
using UnityEngine;

namespace Core.Abilities
{
    public class AnimatorSetTrigger : FindComponent<CharacterAnimator>
    {
        [SerializeField]
        private CharacterAnimator.Variables.AnimatorTrigger _trigger;

        public override string Description => "Set AnimatorTrigger on Caster's/Target's CharacterAnimator component";

        protected override void ExecuteOnUnityComponent(IReadOnlyContext context, CharacterAnimator component) => component.SetTrigger(_trigger);
    }
}