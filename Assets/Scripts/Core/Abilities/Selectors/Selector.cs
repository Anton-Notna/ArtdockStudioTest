using UnityEngine;

namespace Core.Abilities
{
    public abstract class Selector : ScriptableObject 
    {
        public virtual string Description => null;

        public abstract void ValidateSelection(Vector3 rawCastPoint, ISelectionContext context);
    }
}