using UnityEngine;

namespace Core.Abilities
{
    public class AOESelector : Selector
    {
        [SerializeField]
        private LayerMask _mask;
        [SerializeField]
        private float _maxDistanceFromCaster;
        [SerializeField]
        private float _radius;
        [SerializeField]
        private int _collidersLimit = 10;

        private Collider[] _colliders;

        public override string Description => "Select multiple units in Radius.\nCastPoint can be placed in MaxDistanceFromCaster.";

        public override void ValidateSelection(Vector3 rawCastPoint, ISelectionContext context)
        {
            context.ClearTargets();

            if (_colliders == null || _colliders.Length != _collidersLimit)
                _colliders = new Collider[_collidersLimit];

            Vector3 casterPosition = context.Caster.transform.position;
            Vector3 offset = rawCastPoint - casterPosition;
            if (offset.sqrMagnitude > _maxDistanceFromCaster * _maxDistanceFromCaster)
                rawCastPoint = casterPosition + (offset.normalized * _maxDistanceFromCaster);

            context.CastPoint = rawCastPoint;

            int colliders = Physics.OverlapSphereNonAlloc(rawCastPoint, _radius, _colliders, _mask);
            for (int i = 0; i < colliders; i++)
                context.AddTarget(_colliders[i].gameObject);

            context.ValidSelection = true;
        }

        private void OnValidate()
        {
            if (_maxDistanceFromCaster < 0f)
                _maxDistanceFromCaster = 0f;

            if (_radius < 0)
                _radius = 0;

            if (_collidersLimit < 0)
                _collidersLimit = 0;
        }
    }
}