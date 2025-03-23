using System;

namespace Core.Abilities
{
    [Flags]
    public enum PositionTarget : byte
    {
        Caster = 1 << 0,
        CastPoint = 1 << 1,
        Targets = 1 << 2,
    }
}