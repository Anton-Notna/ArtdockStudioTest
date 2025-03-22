using UnityEngine;

namespace Core.Abilities
{
    public class DebugMessage : AbilityComponent
    {
        [SerializeField]
        private string _message = "It's works!";

        protected override float StartExecute(IReadOnlyContext context)
        {
            Debug.Log($"{_message}\nContext.Caster: {context.Caster.name}", context.Caster);
            return default;
        }
    }
}