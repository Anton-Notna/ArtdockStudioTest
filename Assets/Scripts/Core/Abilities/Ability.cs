using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Abilities
{

    [CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Abilities/Ability")]
    public class Ability : ScriptableObject
    {
        [SerializeField]
        private SelectorPreset _selectorPreset;
        [SerializeField]
        private List<AbilityComponent> _components;

        public IEnumerator Select(Selectors selectors, ISelectionContext context)
        {
            if (selectors.GetSelector(_selectorPreset.GetType(), out Selector selector) == false)
                return null;

            return selector.Select(_selectorPreset, context);
        }

        public IEnumerator Execute(IReadOnlyContext context)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                AbilityComponent component = _components[i];

                WaitForSeconds duration = component.Execute(context);
                if (duration != null)
                    yield return duration;
            }
        }
    }
}