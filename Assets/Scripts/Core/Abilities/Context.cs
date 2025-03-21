using System.Collections.Generic;
using UnityEngine;
using Core.Gfx;
using System;

namespace Core.Abilities
{
    public class Context : IReadOnlyContext, ISelectionContext
    {
        private readonly List<GameObject> _targets = new List<GameObject>();

        public Context(GameObject caster, CharacterAnimator animator)
        {
            Caster = caster;
            Animator = animator;
        }

        public GameObject Caster { get; private set; }

        public CharacterAnimator Animator { get; private set; }

        public IReadOnlyList<GameObject> Targets => _targets;

        public Vector3 CastOrigin { get; set; }

        public void AddTarget(GameObject target) => _targets.Add(target);

        public void Reset()
        {
            _targets.Clear();
        }
    }

    public interface ISelectionContext
    {
        public GameObject Caster { get; }

        public Vector3 CastOrigin { get; set; }

        public void AddTarget(GameObject target);
    }
}