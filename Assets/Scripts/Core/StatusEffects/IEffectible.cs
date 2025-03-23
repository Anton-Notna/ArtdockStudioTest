namespace Core.StatusEffects
{
    public interface IEffectible
    {
        public void AddEffect(StatusEffect effect, float duration);

        public void AddPermanentEffect(StatusEffect effect);

        public void RemoveEffect(StatusEffect effect);
    }
}