using Core.Effects.Sounds;
using System;
using UnityEngine;

namespace Core.Abilities
{
    public class AudioClipPlayer : AbilityComponent
    {
        [SerializeField]
        private AudioClip _clip;
        [SerializeField, Range(0f, 1f)]
        private float _volume = 1f;

        public override string Description => "Plays AudioClip in CastOrigin";

        protected override float StartExecute(IReadOnlyContext context)
        {
            _clip.PlayAtPoint(context.CastPoint, _volume);
            return default;
        }
    }
}