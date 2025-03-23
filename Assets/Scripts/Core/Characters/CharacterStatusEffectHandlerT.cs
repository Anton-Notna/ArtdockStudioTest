using Core.StatusEffects;

namespace Core.Characters
{
    public abstract class CharacterStatusEffectHandler<T> : CharacterStatusEffectHandler where T : StatusEffect
    {
        public override bool CanHandle(StatusEffect effect) => effect.GetType() == typeof(T);

        public override void HandleAdd(StatusEffect effect) => HandleAdd((T)effect);

        public override void HandleUpdate(StatusEffect effect) => HandleUpdate((T)effect);

        public override void HandleRemove(StatusEffect effect) => HandleRemove((T)effect);

        protected virtual void HandleAdd(T effect) { }

        protected virtual void HandleUpdate(T effect) { }

        protected virtual void HandleRemove(T effect) { }
    }
}