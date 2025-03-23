using System;

namespace Core.Abilities
{
    [Flags]
    public enum GameObjectTarget : byte
    {
        Caster = 1 << 0,
        Targets = 1 << 1,
    }
}