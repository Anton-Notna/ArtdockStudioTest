using UnityEngine;

namespace Core.Damages
{
    public interface IDamageable
    {
        public bool TakeDamage(Damage damage);
    }
}
