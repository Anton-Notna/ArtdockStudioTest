using UnityEngine;

namespace Core.Effects.Sounds
{
    public static class AudioClipExtensions
    {
        public static void PlayAtPoint(this AudioClip clip, Vector3 position, float volume = 1f) => SoundPlayers.Instance.PlayAtPoint(clip, position, volume);
    }
}