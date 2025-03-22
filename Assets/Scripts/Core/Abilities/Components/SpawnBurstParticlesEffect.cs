using Core.Effects.Vfx;
using System;
using UnityEngine;

namespace Core.Abilities
{
    public class SpawnBurstParticlesEffect : AbilityComponent
    {
        [Flags]
        private enum Target : byte
        {
            Caster = 1 << 0,
            CastPoint = 1 << 1,
            Targets = 1 << 2,
        }

        [SerializeField]
        private BurstParticlesEffect _prefab;
        [SerializeField]
        private Target _target;

        protected override float StartExecute(IReadOnlyContext context)
        {
            if ((_target & Target.Caster) != 0)
                Play(context.Caster);

            if ((_target & Target.CastPoint) != 0)
                Play(context.CastPoint, Quaternion.identity);

            if ((_target & Target.Targets) != 0)
            {
                for (int i = 0; i < context.Targets.Count; i++)
                 Play(context.Targets[i]);
            }

            return default;
        }

        private void Play(GameObject target) 
        {
            if (target != null)
                Play(target.transform.position, target.transform.rotation);
        }

        private void Play(Vector3 position, Quaternion rotation) => GameObject.Instantiate(_prefab).Play(position, rotation);
    }
}