using System;
using System.Collections;
using UnityEngine;

namespace Core.Abilities
{
    public class AbilityExecutor : MonoBehaviour
    {
        private ICastPointSelector _castPoint;
        private Context _context;
        private Coroutine _process;

        public bool Inited => _context != null;

        public bool Running => _process != null;

        public void Setup(ICastPointSelector castPoint)
        {
            if (Inited)
                throw new InvalidOperationException($"AbilityExecutor already inited. GameObject: {gameObject.name}");

            _context = new Context(gameObject);
            _castPoint = castPoint;
        }

        public void Clear()
        {
            if (Inited == false)
                return;

            Stop();
            _context = null;
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
            _castPoint.Deactivate();
            _context.Reset();
        }

        private IEnumerator Process(Ability ability)
        {
            _castPoint.Activate();
            yield return ability.Select(_castPoint, _context);
            _castPoint.Deactivate();

            if (_context.ValidSelection)
                yield return ability.Execute(_context);

            Stop();
        }
    }
}