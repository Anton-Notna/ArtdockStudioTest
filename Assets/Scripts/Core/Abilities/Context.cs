using System.Collections.Generic;
using UnityEngine;
using Core.Gfx;

namespace Core.Abilities
{
    public class Context : IReadOnlyContext
    {
        public GameObject Caster { get; private set; }

        public IReadOnlyList<GameObject> Targets { get; private set; }

        public Vector3 CastOrigin { get; private set; }

        public CharacterAnimator Animator { get; private set; }
}
}