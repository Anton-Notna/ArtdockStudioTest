using Core.Effects.Vfx;
using UnityEngine;

namespace Core.Abilities
{
    public class SpawnBurstParticlesEffect : AbilityComponent
    {
        [SerializeField]
        private BurstParticlesEffect _prefab;
        [SerializeField]
        private PositionTarget _target;
        [SerializeField]
        private Vector3 _localOffset;

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

        private void Play(GameObject target) 
        {
            if (target != null)
                Play(target.transform.TransformPoint(_localOffset), target.transform.rotation);
        }

        private void Play(Vector3 position, Quaternion rotation) => GameObject.Instantiate(_prefab).Play(position, rotation);
    }
}