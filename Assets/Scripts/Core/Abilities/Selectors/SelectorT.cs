using System;
using System.Collections;

namespace Core.Abilities
{
    public abstract class Selector<T> : Selector where T : SelectorPreset
    {
        public override Type PresetType => typeof(T);

        public override IEnumerator Select(SelectorPreset preset, ISelectionContext selection) => Select((T)preset, selection);

        protected abstract IEnumerator Select(T preset, ISelectionContext selection);
    }
}