using Core.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Characters
{
    public class PlayerCharacterControl : MonoBehaviour
    {
        [SerializeField]
        private AbilityExecutor _executor;
        [SerializeField]
        private Ability _attackAbility;
        [SerializeField]
        private Ability _dashAbility;
        [SerializeField]
        private Ability _primaryAbility;
        [SerializeField]
        private CharacterMotor _motor;

        private bool _inited;
        private Transform _camera;
        private InputSystemActions _input;
        private InputAction _move;
        private InputAction _attack;
        private InputAction _dash;
        private InputAction _primary;

        public void Setup(Transform camera)
        {
            if (_inited)
                Clear();

            _camera = camera;

            _input = new InputSystemActions();
            _move = _input.Player.Move;
            _move.Enable();

            _attack = _input.Player.Attack;
            _attack.Enable();
            _attack.performed += Attack;

            _dash = _input.Player.Dash;
            _dash.Enable();
            _dash.performed += Dash;

            _primary = _input.Player.PrimaryAbility;
            _primary.Enable();
            _primary.performed += UsePrimary;

            _inited = true;
        }

        public void Clear()
        {
            if (_inited == false)
                return;

            _move.Disable();

            _attack.Disable();
            _attack.performed -= Attack;

            _dash.Disable();
            _dash.performed -= Dash;

            _primary.Disable();
            _primary.performed -= UsePrimary;
        }

        private void Update()
        {
            if (_inited == false)
                return;

            Vector2 input = _move.ReadValue<Vector2>();
            Vector3 forward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;
            Vector3 motionDirection = forward * input.y + (right * input.x);
            _motor.SetMotionDirection(motionDirection);
        }

        private void UsePrimary(InputAction.CallbackContext context)
        {
            _executor.Run(_primaryAbility);
        }

        private void Dash(InputAction.CallbackContext context)
        {
            _executor.Run(_dashAbility);
        }

        private void Attack(InputAction.CallbackContext context)
        {
            _executor.Run(_attackAbility);
        }

    }
}