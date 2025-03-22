using System.Collections.Generic;
using UnityEngine;

namespace Core.Abilities
{
    public interface ISelectionContext
    {
        public GameObject Caster { get; }

        public bool ValidSelection { get; set; }

        public Vector3 CastPoint { get; set; }

        public void ClearTargets();

        public void AddTarget(GameObject target);
    }
}