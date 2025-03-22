using UnityEngine;

namespace Core.Effects.Vfx
{
    public class BurstParticlesEffect : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] _particleSystems;
        [SerializeField]
        private float _lifeTime = 5f;

        public void Play(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            for (int i = 0; i < _particleSystems.Length; i++)
            {
                var particleSystem = _particleSystems[i];
                int amount = Mathf.RoundToInt(particleSystem.emission.GetBurst(0).count.constant);
                if (amount > 0)
                    particleSystem.Emit(amount);
            }

            Destroy(gameObject, _lifeTime);
        }

        private void OnValidate()
        {
            if (_lifeTime < 0f)
                _lifeTime = 0f;
        }
    }
}