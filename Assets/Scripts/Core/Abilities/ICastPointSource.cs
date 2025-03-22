using UnityEngine;

namespace Core.Abilities
{
    public interface ICastPointSource
    {
        public Vector3 GetRawCastOrigin();
    }
}