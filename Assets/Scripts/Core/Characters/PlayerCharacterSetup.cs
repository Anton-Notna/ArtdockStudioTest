using Core.Abilities;
using Core.Gfx;
using UnityEngine;

namespace Core.Characters
{
    public class PlayerCharacterSetup : MonoBehaviour
    {
        [SerializeField]
        private PlayerCharacterControl _input;
        [SerializeField]
        private CharacterAnimator _animator;
        [SerializeField]
        private CharacterMotor _motor;
        [SerializeField]
        private AbilityExecutor _abilityExecutor;

        private bool _inited;

        private class DummyCastPointSelector : ICastPointSelector
        {
            public Transform t;

            public void Activate()
            {
                
            }

            public void Deactivate()
            {
                
            }

            public Vector3 GetRawCastOrigin()
            {
                return t.TransformPoint(Vector3.forward * 3);
            }
        }

        public void Setup(Transform camera)
        {
            if (_inited)
                Clear();

            _input.Setup(camera);
            _abilityExecutor.Setup(new DummyCastPointSelector() { t = transform });
            _inited = true;
        }

        public void Clear()
        {
            if (_inited == false)
                return;

            _input.Clear();
            _abilityExecutor.Clear();
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