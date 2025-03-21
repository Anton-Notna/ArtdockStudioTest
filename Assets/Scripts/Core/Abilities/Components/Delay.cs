using UnityEngine;

namespace Core.Abilities
{
    [CreateAssetMenu(fileName = "WaitDuration", menuName = "Scriptable Objects/Abilities/Components/WaitDuration")]
    public class Delay : AbilityComponent
    {
        [SerializeField]
        private float _duration = 1f;

        protected override float StartExecute(IReadOnlyContext context) => _duration;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_duration < 0f)
                _duration = 0f;
        }
#endif
    }
}