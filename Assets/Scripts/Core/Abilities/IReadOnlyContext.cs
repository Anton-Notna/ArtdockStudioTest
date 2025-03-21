using System.Collections.Generic;
using UnityEngine;
using Core.Gfx;

namespace Core.Abilities
{
    public interface IReadOnlyContext
    {
        public GameObject Caster { get; }

        public CharacterAnimator Animator { get; }

        public IReadOnlyList<GameObject> Targets { get; }

        public Vector3 CastOrigin { get; }
    }
}