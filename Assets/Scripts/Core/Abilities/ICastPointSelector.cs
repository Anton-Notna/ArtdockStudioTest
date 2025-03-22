namespace Core.Abilities
{
    public interface ICastPointSelector : ICastPointSource
    {
        public void Activate();

        public void Deactivate();
    }
}