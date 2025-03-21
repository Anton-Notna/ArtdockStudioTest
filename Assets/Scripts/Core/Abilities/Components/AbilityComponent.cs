using UnityEngine;

namespace Core.Abilities
{
    public abstract class AbilityComponent : ScriptableObject
    {
        private WaitForSeconds _cashed;
        private float _cashedDuration;

        public WaitForSeconds Execute(IReadOnlyContext context)
        {
            float duration = StartExecute(context);

            if (duration == 0f)
                return null;

            if (_cashed == null || duration != _cashedDuration)
            {
                _cashed = new WaitForSeconds(duration);
                _cashedDuration = duration;
            }

            return _cashed;
        }

        protected abstract float StartExecute(IReadOnlyContext context);
    }
}