using UnityEngine;

namespace Core.Gfx
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public void SetTrigger(AnimatorTrigger trigger) => _animator.SetTrigger(trigger.ToString());
    }
}