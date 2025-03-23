using UnityEngine;

namespace Core.StatusEffects
{
    [CreateAssetMenu(fileName = "SlowMotionEffect", menuName = "Scriptable Objects/Status Effects/SlowMotionEffect")]
    public class SlowMotionEffect : StatusEffect
    {
        [SerializeField, Range(0f, 1f)]
        private float _speedRatio = 0.5f;

        public float SpeedRatio => _speedRatio;
    }
}