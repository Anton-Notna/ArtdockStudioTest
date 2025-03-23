using UnityEngine;

namespace Core.StatusEffects
{
    [CreateAssetMenu(fileName = "ContinuousDamageEffect", menuName = "Scriptable Objects/Status Effects/ContinuousDamageEffect")]
    public class ContinuousDamageEffect : StatusEffect
    {
        // Time of 1 frame in 60 FPS.
        private const float _minCycleDelay = 0.016f;

        [SerializeField]
        private int _damagePrCycle = 1;
        [SerializeField]
        private float _cycleDelay = 0.1f;

        public int DamagePerCycle => _damagePrCycle;

        public float CycleDelay => _cycleDelay;

        private void OnValidate()
        {
            if (_damagePrCycle < 0f)
                _damagePrCycle = 0;

            if (_cycleDelay < _minCycleDelay)
                _cycleDelay = _minCycleDelay;
        }
    }
}