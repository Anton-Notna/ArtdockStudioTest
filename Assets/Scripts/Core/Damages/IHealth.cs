using System;

namespace Core.Damages
{
    public interface IHealth : IDamageable
    {
        public int Health { get; }

        public event Action<Damage, IHealth> DamageTaken;
    }
}
