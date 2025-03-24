using Core.Abilities;
using Core.Gfx;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Characters
{
    public class PlayerCharacterSetup : MonoBehaviour
    {
        [SerializeField]
        private PlayerCharacterControl _input;
        [SerializeField]
        private PlayerCastPointSelector _selector;
        [SerializeField]
        private CharacterAnimator _animator;
        [SerializeField]
        private CharacterMotor _motor;
        [SerializeField]
        private AbilityExecutor _abilityExecutor;
        [SerializeField]
        private CharacterAbilities _abilities;

        private bool _inited;

        public void Setup(Camera camera, IEnumerable<KeyValuePair<CharacterAbilities.Slot, Ability>> abilities)
        {
            if (_inited)
                Clear();

            _abilityExecutor.Setup(_selector);

            foreach (var item in abilities)
                _abilities.Replace(item.Key, item.Value);

            _input.Setup(camera.transform);
            _selector.Setup(camera);
            _selector.Activated += _input.DisableInput;
            _selector.Deactivated += _input.EnableInput;

            _input.EnableInput();

            _inited = true;
        }

        public void Clear()
        {
            if (_inited == false)
                return;

            _selector.Clear();
            _input.Clear();
            _abilityExecutor.Clear();
            _selector.Activated -= _input.DisableInput;
            _selector.Deactivated -= _input.EnableInput;
        }

        private void Update()
        {
            if (_inited == false)
                return;

            _animator.SetSpeed(_motor.NormalizedSpeed);
        }

        private void OnDestroy() => Clear();
    }
}