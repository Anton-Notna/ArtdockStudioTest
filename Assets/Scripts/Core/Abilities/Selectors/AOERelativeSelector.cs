using UnityEngine;

namespace Core.Abilities
{
    public class AOERelativeSelector : Selector
    {
        [SerializeField]
        private LayerMask _mask;
        [SerializeField]
        private Vector3 _relativeOffset;
        [SerializeField]
        private float _radius;
        [SerializeField]
        private int _collidersLimit = 10;

        private Collider[] _colliders;

        public override bool Momentum => true;

        public override string Description => "Select multiple units in Radius.\nCastPoint will be relative to Caster.transform";

        public override void ValidateSelection(Vector3 rawCastPoint, ISelectionContext context)
        {
            context.ClearTargets();

            if (_colliders == null || _colliders.Length != _collidersLimit)
                _colliders = new Collider[_collidersLimit];

            context.CastPoint = context.Caster.transform.TransformPoint(_relativeOffset);
            int colliders = Physics.OverlapSphereNonAlloc(context.CastPoint, _radius, _colliders, _mask);
            for (int i = 0; i < colliders; i++)
                context.AddTarget(_colliders[i].gameObject);

            context.ValidSelection = true;
        }

        private void OnValidate()
        {
            if (_radius < 0)
                _radius = 0;

            if (_collidersLimit < 0)
                _collidersLimit = 0;
        }
    }
}