using UnityEngine;

namespace Core.Abilities
{
    public class CasterSelector : Selector
    {
        public override string Description => "Set Caster as a Target and Caster's position as a CastOrigin.";

        public override bool Momentum => true;

        public override void ValidateSelection(Vector3 rawCastPoint, ISelectionContext context)
        {
            context.CastPoint = context.Caster.transform.position;
            context.ClearTargets();
            context.AddTarget(context.Caster);
            context.ValidSelection = true;
        }
    }
}