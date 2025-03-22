using System.Collections.Generic;
using UnityEngine;

namespace Core.Abilities
{
    public class Context : IReadOnlyContext, ISelectionContext
    {
        private readonly List<GameObject> _targets = new List<GameObject>();

        public Context(GameObject caster) => Caster = caster;

        public GameObject Caster { get; private set; }

        public bool ValidSelection { get; set; }

        public IReadOnlyList<GameObject> Targets => _targets;

        public Vector3 CastPoint { get; set; }

        public void ClearTargets() => _targets.Clear();

        public void AddTarget(GameObject target) => _targets.Add(target);

        public void Reset()
        {
            ClearTargets();
            ValidSelection = false;
        }
    }
}