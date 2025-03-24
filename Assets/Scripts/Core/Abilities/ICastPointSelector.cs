namespace Core.Abilities
{
    public interface ICastPointSelector : ICastPointSource
    {
        public bool Active { get; }

        public void Activate();

        public void Deactivate();
    }
}