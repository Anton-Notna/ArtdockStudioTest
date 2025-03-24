using UnityEngine;

namespace Core.Abilities
{
    public abstract class Selector : ScriptableObject 
    {
        public abstract bool Momentum { get; }

        public virtual string Description => null;

        public abstract void ValidateSelection(Vector3 rawCastPoint, ISelectionContext context);
    }
}