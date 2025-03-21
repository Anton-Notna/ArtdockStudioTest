using UnityEngine;

namespace Core.Abilities
{
    [CreateAssetMenu(fileName = "UnitSelectorPreset", menuName = "Scriptable Objects/Abilities/Selectors/UnitSelectorPreset")]
    public class UnitSelectorPreset : SelectorPreset
    {
        [SerializeField]
        private LayerMask _mask;
        [SerializeField]
        private float _radius;
    
        public LayerMask LayerMask => _mask;

        public float Radius => _radius;

    }
}