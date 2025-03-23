using Core.Characters;
using UnityEngine;

namespace Core.Boot
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private PlayerCharacterSetup _playerPrefab;
        [SerializeField]
        private Transform _playerSpawnPoint;

        private void Start() => Bootstrap();

        private void Bootstrap()
        {
            PlayerCharacterSetup player = Instantiate(_playerPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation);
            player.Setup(_camera.transform);
        }
    }
}