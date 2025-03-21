using System.Collections;
using UnityEngine;

namespace Core.Abilities
{
    public class SelfSelector : Selector<SelfSelectorPreset>
    {
        protected override IEnumerator Select(SelfSelectorPreset preset, ISelectionContext selection)
        {
            selection.AddTarget(selection.Caster);
            selection.CastOrigin = selection.Caster.transform.position;
            yield break;
        }
    }
}