using Core.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Characters
{
    public class CharacterAbilities : MonoBehaviour
    {
        public enum Slot
        {
            Attack = 0,
            Dash = 1,
            Primary = 2,
        }

        [SerializeField]
        private AbilityExecutor _executor;

        private readonly Dictionary<Slot, Ability> _abilities = new Dictionary<Slot, Ability>();

        public void Replace(Slot slot, Ability ability) => _abilities[slot] = ability;

        public void Use(Slot slot)
        {
            if (_executor.Running)
                return;

            if (_abilities.TryGetValue(slot, out Ability ability) == false)
                return;

            _executor.Run(ability);
        }
    }
}