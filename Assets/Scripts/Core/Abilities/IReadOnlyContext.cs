using System.Collections.Generic;
using UnityEngine;

namespace Core.Abilities
{
    public interface IReadOnlyContext
    {
        public GameObject Caster { get; }

        public IReadOnlyList<GameObject> Targets { get; }

        public Vector3 CastPoint { get; }
    }
}