using Core.Gfx;
using System;
using System.Collections;
using UnityEngine;

namespace Core.Abilities
{
    public class AbilityExecutor : MonoBehaviour
    {
        [SerializeField]
        private Selectors _selectors;
        [SerializeField]
        private GameObject _caster;
        [SerializeField]
        private CharacterAnimator _animator;

        private Context _context;
        private Coroutine _process;

        public bool Inited => _context != null;

        public bool Running => _process != null;

        public void Init()
        {
            if (Inited)
                throw new InvalidOperationException($"AbilityExecutor already inited. GameObject: {gameObject.name}");

            _context = new Context(_caster, _animator);
        }

        public void Run(Ability ability)
        {
            if (Inited == false)
                throw new InvalidOperationException($"AbilityExecutor don't inited. GameObject: {gameObject.name}");

            if (Running)
                throw new InvalidOperationException($"AbilityExecutor already running. GameObject: {gameObject.name}");

            _process = StartCoroutine(Process(ability));
        }

        public void Stop()
        {
            if (Inited == false)
                throw new InvalidOperationException($"AbilityExecutor don't inited. GameObject: {gameObject.name}");

            if (Running == false)
                return;

            StopCoroutine(_process);
            _process = null;
            _selectors.StopSelection();
            _context.Reset();
        }

        private IEnumerator Process(Ability ability)
        {
            yield return ability.Select(_selectors, _context);
            yield return ability.Execute(_context);
            Stop();
        }
    }
}