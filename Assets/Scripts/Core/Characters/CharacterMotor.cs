﻿using Core.StatusEffects;
using System;
using UnityEngine;

namespace Core.Characters
{
    public class CharacterMotor : CharacterStatusEffectHandler<SlowMotionEffect>
    {
        [Serializable]
        private class SmoothDampVector
        {
            [SerializeField]
            private float _smooth;

            private Vector3 _current;
            private Vector3 _target;
            private Vector3 _velocity;

            public Vector3 Current => _current;

            public void Reset(Vector3 current)
            {
                _current = current;
                _target = current;
                _velocity = Vector3.zero;
            }

            public void SetTarget(Vector3 target) => _target = target;

            public void Update(float deltaTime) => _current = Vector3.SmoothDamp(_current, _target, ref _velocity, _smooth, Mathf.Infinity, deltaTime);

            public void OnValidate()
            {
                if (_smooth < 0f)
                    _smooth = 0f;
            }
        }

        [Serializable]
        private class SmoothDampAngle
        {
            [SerializeField]
            private float _smooth;

            private float _current;
            private float _target;
            private float _velocity;

            public float Current => _current;

            public void Reset(float current)
            {
                _current = current;
                _target = current;
                _velocity = 0f;
            }

            public void SetTarget(float target) => _target = target;

            public void Update(float deltaTime) => _current = Mathf.SmoothDampAngle(_current, _target, ref _velocity, _smooth, Mathf.Infinity, deltaTime);

            public void OnValidate()
            {
                if (_smooth < 0f)
                    _smooth = 0f;
            }
        }

        [SerializeField]
        private CharacterController _controller;
        [SerializeField]
        private float _speed = 5f;
        [SerializeField]
        private float _gravity = 20f;
        [SerializeField]
        private SmoothDampVector _motion = new SmoothDampVector();
        [SerializeField]
        private SmoothDampAngle _rotation = new SmoothDampAngle();
        [SerializeField]
        private SmoothDampVector _impulse = new SmoothDampVector();

        private Vector3 _motionDirection;
        private float? _speedRatio;

        public float NormalizedSpeed { get; private set; }

        public void AddImpulse(Vector3 impulse)
        {
            _impulse.Reset(_impulse.Current + impulse);
            _impulse.SetTarget(Vector3.zero);
        }

        public void SetMotionDirection(Vector3 direction)
        {
            if (direction.sqrMagnitude < Mathf.Epsilon)
                _motionDirection = Vector3.zero;
            else
                _motionDirection = direction.normalized;
        }

        protected override void HandleAdd(SlowMotionEffect effect) => _speedRatio = effect.SpeedRatio;

        protected override void HandleRemove(SlowMotionEffect effect) => _speedRatio = null;

        private void Update()
        {
            _impulse.Update(Time.deltaTime);

            Vector3 motion = _motionDirection * _speed;
            if (_speedRatio.HasValue)
                motion *= _speedRatio.Value;

            NormalizedSpeed = motion.magnitude / _speed;

            motion += Vector3.down * _gravity;
            motion += _impulse.Current;

            _motion.SetTarget(motion);
            _motion.Update(Time.deltaTime);

            Vector3 lookDirection = Vector3.ProjectOnPlane(_motionDirection, Vector3.up);
            if (lookDirection.sqrMagnitude > Mathf.Epsilon)
            {
                float rotation = Vector3.SignedAngle(Vector3.forward, lookDirection.normalized, Vector3.up);
                _rotation.SetTarget(rotation);
            }

            _rotation.Update(Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.up * _rotation.Current);

            _controller.Move(motion * Time.deltaTime);
        }

        private void OnValidate()
        {
            if (_speed < 0f)
                _speed = 0f;

            if (_gravity < 0f)
                _gravity = 0f;

            _motion?.OnValidate();
            _impulse?.OnValidate();
            _rotation?.OnValidate();
        }
    }
}