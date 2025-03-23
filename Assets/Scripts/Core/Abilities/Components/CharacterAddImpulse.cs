using Core.Characters;
using System;
using UnityEngine;

namespace Core.Abilities
{
    public class CharacterAddImpulse : AbilityComponent
    {
        private enum Case
        {
            LocalForward = 0,
            ToCastPoint = 1,
            FromCastPoint = 2,
        }

        [SerializeField]
        private GameObjectTarget _target;
        [SerializeField]
        private Case _case;
        [SerializeField]
        private float _impulseMagnitude;

        public override string Description => "Adds impulse on Caster's/Target's CharacterMotor component";

        protected override float StartExecute(IReadOnlyContext context)
        {
            if ((_target & GameObjectTarget.Caster) != 0)
                AddImpulse(context.CastPoint, context.Caster);

            if ((_target & GameObjectTarget.Targets) != 0)
            {
                for (int i = 0; i < context.Targets.Count; i++)
                    AddImpulse(context.CastPoint, context.Targets[i]);
            }

            return default;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_impulseMagnitude < 0f)
                _impulseMagnitude = 0f;
        }
#endif

        private void AddImpulse(Vector3 castPoint, GameObject gameObject)
        {
            if (gameObject == null)
                return;

            if (gameObject.TryGetComponent(out CharacterMotor motor) == false)
                return;

            Vector3 direction = _case switch
            {
                Case.LocalForward => gameObject.transform.forward,
                Case.FromCastPoint => (gameObject.transform.position - castPoint).normalized,
                Case.ToCastPoint => (castPoint - gameObject.transform.position).normalized,
                _ => throw new NotImplementedException(),
            };

            motor.AddImpulse(direction * _impulseMagnitude);
        }
    }
}