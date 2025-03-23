using Core.Effects.Vfx;
using UnityEngine;

namespace Core.Abilities
{
    public class SpawnContinuousParticlesEffect : AbilityComponent
    {
        [SerializeField]
        private ContinuousParticlesEffect _prefab;
        [SerializeField]
        private PositionTarget _target;
        [SerializeField]
        private Vector3 _localOffset;
        [SerializeField]
        private bool _attachToTarget;
        [SerializeField]
        private float _effectsLifeTime;

        protected override float StartExecute(IReadOnlyContext context)
        {
            if ((_target & PositionTarget.Caster) != 0)
                Play(context.Caster);

            if ((_target & PositionTarget.CastPoint) != 0)
                Play(context.CastPoint + _localOffset, Quaternion.identity);

            if ((_target & PositionTarget.Targets) != 0)
            {
                for (int i = 0; i < context.Targets.Count; i++)
                    Play(context.Targets[i]);
            }

            return default;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_effectsLifeTime < 0f)
                _effectsLifeTime = 0f;
        }
#endif

        private void Play(GameObject target)
        {
            if (target == null)
                return;

            Transform parent = _attachToTarget ? target.transform : null;
            GameObject.Instantiate(_prefab).Play(target.transform.TransformPoint(_localOffset), target.transform.rotation, _effectsLifeTime, parent);
        }

        private void Play(Vector3 position, Quaternion rotation) => GameObject.Instantiate(_prefab).Play(position, rotation, _effectsLifeTime);
    }
}