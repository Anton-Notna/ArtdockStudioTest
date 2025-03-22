using UnityEngine;

namespace Core.Effects.Sounds
{
    public class SoundPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private float _releaseTime = float.MinValue;

        public bool Free => Time.time > _releaseTime;

        private void OnEnable()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = false;
        }

        public void PlayAtPoint(AudioClip clip, Vector3 position, float volume)
        {
            transform.position = position;
            _audioSource.clip = clip;
            _audioSource.volume = volume;

            _audioSource.Play();
            _releaseTime = Time.time + _audioSource.clip.length;
        }
    }
}