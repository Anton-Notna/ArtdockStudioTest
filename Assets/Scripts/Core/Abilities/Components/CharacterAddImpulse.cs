using Core.Characters;
using System;
using UnityEngine;

namespace Core.Abilities
{
    public class CharacterAddImpulse : FindComponent<CharacterMotor>
    {
        private enum Case
        {
            LocalForward = 0,
            ToCastPoint = 1,
            FromCastPoint = 2,
            ToCaster = 3,
            FromCaster = 4,
        }

        [SerializeField]
        private Case _case;
        [SerializeField]
        private float _impulseMagnitude;

        public override string Description => "Adds impulse on Caster's/Target's CharacterMotor component";

        protected override void ExecuteOnUnityComponent(IReadOnlyContext context, CharacterMotor motor)
        {
            Vector3 direction = _case switch
            {
                Case.LocalForward => motor.transform.forward,
                Case.FromCastPoint => (motor.transform.position - context.CastPoint).normalized,
                Case.ToCastPoint => (context.CastPoint - motor.transform.position).normalized,
                Case.FromCaster => (motor.transform.position - context.Caster.transform.position).normalized,
                Case.ToCaster => (context.Caster.transform.position - motor.transform.position).normalized,
                _ => throw new NotImplementedException(),
            };

            motor.AddImpulse(direction * _impulseMagnitude);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_impulseMagnitude < 0f)
                _impulseMagnitude = 0f;
        }
#endif
    }
}