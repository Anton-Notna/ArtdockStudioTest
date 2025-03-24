using UnityEngine;

namespace Core.Abilities
{
    public class SoloUnitSelector : Selector
    {
        [SerializeField]
        private LayerMask _mask;
        [SerializeField]
        private float _maxDistanceFromCaster;
        [SerializeField]
        private int _collidersLimit = 10;
    
        private Collider[] _colliders;

        public override string Description => "Select single unit closest to CastPoint.\nCastPoint can be placed in MaxDistanceFromCaster.";

        public override bool Momentum => false;

        public override void ValidateSelection(Vector3 rawCastPoint, ISelectionContext context)
        {
            context.ClearTargets();

            if (_colliders == null || _colliders.Length != _collidersLimit)
                _colliders = new Collider[_collidersLimit];

            int colliders = Physics.OverlapSphereNonAlloc(context.Caster.transform.position, _maxDistanceFromCaster, _colliders, _mask);

            context.ValidSelection = colliders > 0;
            if (context.ValidSelection == false)
                return;
            
            float minSqrDistance = float.MaxValue;
            Collider closest = null;

            for (int i = 0; i < colliders; i++)
            {
                Collider collider = _colliders[i];
                float sqrDistance = (collider.transform.position - rawCastPoint).sqrMagnitude;

                if (sqrDistance >= minSqrDistance)
                    continue;

                closest = collider;
                minSqrDistance = sqrDistance;
            }

            context.AddTarget(closest.gameObject);
            context.CastPoint = closest.transform.position;
            context.ValidSelection = true;
        }

        private void OnValidate()
        {
            if (_maxDistanceFromCaster < 0f)
                _maxDistanceFromCaster = 0f;

            if (_collidersLimit < 0)
                _collidersLimit = 0;
        }
    }
}