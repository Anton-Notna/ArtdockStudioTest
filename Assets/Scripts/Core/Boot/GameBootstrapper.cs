using Core.Abilities;
using Core.Characters;
using System.Collections.Generic;
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
        [SerializeField]
        private Ability _attackAbility;
        [SerializeField]
        private Ability _dashAbility;
        [SerializeField]
        private Ability _primaryAbility;

        private void Start() => Bootstrap();

        private void Bootstrap()
        {
            PlayerCharacterSetup player = Instantiate(_playerPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation);
            player.Setup(_camera, new Dictionary<CharacterAbilities.Slot, Ability>() 
            { 
                { CharacterAbilities.Slot.Dash, _dashAbility },
                { CharacterAbilities.Slot.Attack, _attackAbility },
                { CharacterAbilities.Slot.Primary, _primaryAbility },
            });
        }
    }
}