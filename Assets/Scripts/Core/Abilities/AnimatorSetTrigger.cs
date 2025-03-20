using Core.Gfx;
using UnityEngine;

namespace Core.Abilities
{
    [CreateAssetMenu(fileName = "AnimatorSetTrigger", menuName = "Scriptable Objects/Abilities/Components/AnimatorSetTrigger")]
    public class AnimatorSetTrigger : AbilityComponent
    {
        [SerializeField]
        private AnimatorTrigger _trigger;

        protected override float StartExecute(Context context)
        {
            context.Animator.SetTrigger(_trigger);
            return default;
        }
    }
}