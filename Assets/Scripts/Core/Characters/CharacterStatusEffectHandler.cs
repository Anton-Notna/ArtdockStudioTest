using Core.StatusEffects;
using UnityEngine;

namespace Core.Characters
{
    public abstract class CharacterStatusEffectHandler : MonoBehaviour
    {
        public abstract bool CanHandle(StatusEffect effect);

        public abstract void HandleAdd(StatusEffect effect);

        public abstract void HandleUpdate(StatusEffect effect);

        public abstract void HandleRemove(StatusEffect effect);
    }
}