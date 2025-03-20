using UnityEngine;

namespace Core.Abilities
{
    [CreateAssetMenu(fileName = "DebugMessage", menuName = "Scriptable Objects/Abilities/Components/DebugMessage")]
    public class DebugMessage : AbilityComponent
    {
        [SerializeField]
        private string _message = "It's works!";

        protected override float StartExecute(Context context)
        {
            Debug.Log($"{_message}\nContext.Caster: {context.Caster.name}", context.Caster);
            return default;
        }
    }
}