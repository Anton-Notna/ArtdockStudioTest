using UnityEngine;

namespace Core.Gfx
{
    public class CharacterAnimator : MonoBehaviour
    {
        public static class Variables
        {
            public enum AnimatorTrigger
            {
                Unknown = 0,
                MeleeAttack = 1,
                CastSpell = 2,
                CastSpellAOE = 3,
                Roll = 4,
                Impact = 5,
            }

            public const string Speed = "Speed";
        }

        [SerializeField]
        private Animator _animator;
        
        public void SetTrigger(Variables.AnimatorTrigger trigger) => _animator.SetTrigger(trigger.ToString());

        public void SetSpeed(float normalizedSpeed) => _animator.SetFloat(Variables.Speed, normalizedSpeed);
    }
}