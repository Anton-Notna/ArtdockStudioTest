using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Abilities/Ability")]
    public class Ability : ScriptableObject
    {
        [SerializeField]
        private List<AbilityComponent> _components;

        public IEnumerator Execute(Context context)
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