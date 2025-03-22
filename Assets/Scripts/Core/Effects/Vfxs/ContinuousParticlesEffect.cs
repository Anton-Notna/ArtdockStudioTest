using UnityEngine;

namespace Core.Effects.Vfx
{
    public class ContinuousParticlesEffect : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] _particleSystems;

        private bool _followParent;
        private Transform _parent;
        private Vector3 _relativePosition;
        private Quaternion _relativeRotation;

        public void Play(Vector3 position, Quaternion rotation, float lifeTime, Transform parent = null)
        {
            if (_parent == null)
            {
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                _parent = parent;
                _relativePosition = _parent.InverseTransformPoint(position);
                _relativeRotation = Quaternion.Inverse(_parent.rotation) * rotation;
                _followParent = true;
                AlignToParent();
            }

            for (int i = 0; i < _particleSystems.Length; i++)
                _particleSystems[i].Play(false);

            Destroy(gameObject, lifeTime);
        }

        private void LateUpdate()
        {
            if (_followParent == false)
                return;

            if (_parent == null)
            {
                Destroy(gameObject);
                return;
            }

            AlignToParent();
        }

        private void AlignToParent()
        {
            transform.position = _parent.TransformPoint(_relativePosition);
            transform.rotation = _parent.rotation * _relativeRotation;
        }
    }
}