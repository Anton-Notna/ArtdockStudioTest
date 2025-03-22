using System.Collections.Generic;
using UnityEngine;

namespace Core.Effects.Sounds
{
    public class SoundPlayers : MonoBehaviour
    {
        private static SoundPlayers _instance;
        private readonly List<SoundPlayer> _players = new List<SoundPlayer>();

        public static SoundPlayers Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(nameof(SoundPlayers)).AddComponent<SoundPlayers>();
                    GameObject.DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        public void PlayAtPoint(AudioClip clip, Vector3 position, float volume)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                SoundPlayer player = _players[i];
                if (player.Free)
                {
                    player.PlayAtPoint(clip, position, volume);
                    return;
                }
            }

            SoundPlayer newPlayer = new GameObject(nameof(SoundPlayer)).AddComponent<SoundPlayer>();
            newPlayer.transform.parent = transform;
            _players.Add(newPlayer);
            newPlayer.PlayAtPoint(clip, position, volume);
        }
    }
}