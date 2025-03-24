using Core.Abilities;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Core.Characters
{
    public class PlayerCastPointSelector : MonoBehaviour, ICastPointSelector
    {
        [SerializeField]
        private float _castPlaneY;
        [SerializeField]
        private UnityEvent _activated;
        [SerializeField]
        private UnityEvent _deactivated;

        private bool _inited;
        private InputSystemActions _input;
        private InputAction _select;
        private InputAction _cancel;
        private InputAction _position;
        private Camera _camera;
        private Vector2 _screenPosition;
        private bool _active;

        public event Action Activated
        {
            add => _activated.AddListener(value.Invoke);
            remove => _activated.RemoveListener(value.Invoke);
        }

        public event Action Deactivated
        {
            add => _deactivated.AddListener(value.Invoke);
            remove => _deactivated.RemoveListener(value.Invoke);
        }

        public bool Active => _active;

        public void Setup(Camera camera) 
        {
            if (_inited)
                Clear();

            _camera = camera;
            _input = new InputSystemActions();
            _select = _input.Player.Select;
            _select.performed += Deactivate;
            _cancel = _input.Player.ExitSelection;
            _select.performed += Deactivate;
            _position = _input.Player.SelectionPosition;
            _position.performed += ReadScreenPosition;
            _inited = true;
        }

        public void Clear()
        {
            if (_inited == false)
                return;

            Deactivate();
            _select.performed -= Deactivate;
            _cancel.performed -= Deactivate;
            _position.performed -= ReadScreenPosition;

            _inited = false;
        }

        public void Activate()
        {
            if (_inited == false)
                throw new InvalidOperationException("PlayerCastPointSelector not inited.");

            if (_active)
                return;

            _select.Enable();
            _cancel.Enable();
            _position.Enable();

            _active = true;
            _activated.Invoke();
        }

        public void Deactivate()
        {
            if (_inited == false)
                throw new InvalidOperationException("PlayerCastPointSelector not inited.");

            if (_active == false)
                return;

            _select.Disable();
            _cancel.Disable();
            _position.Disable();

            _active = false;
            _deactivated.Invoke();
        }

        public Vector3 GetRawCastOrigin()
        {
            if (_inited == false)
                throw new InvalidOperationException("PlayerCastPointSelector not inited.");

            Ray ray = _camera.ScreenPointToRay(_screenPosition);
            Plane plane = new Plane(Vector3.up, _castPlaneY);
            plane.Raycast(ray, out float enter);

            return ray.GetPoint(enter);
        }

        private void ReadScreenPosition(InputAction.CallbackContext context) => _screenPosition = context.ReadValue<Vector2>();

        private void Deactivate(InputAction.CallbackContext _) => Deactivate();
    }
}