using Assets.Scripts.NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    public class Rock : Projectile, IHasPosition, IMoveTarget
    {
        [SerializeField] private float _aoeRadius;

        private readonly Collider[] _explosionOverlapBuffer = new Collider[5];
        private readonly RaycastHit[] _rayBuffer = new RaycastHit[5];

        private Transform _container;
        private SphereCollider _collider;

        public Vector3 Position => transform.position;

        protected override void OnStarted()
        {
            _collider = (SphereCollider)Collider;
        }

        private void LateUpdate()
        {
            if (_container)
                transform.position =
                    _container.transform.position;
        }

        public void Take(Transform container)
        {
            _container = container;
        }

        public void Throw(Vector3 direction)
        {
            _container = null;
            transform.forward = direction;
            Launch();
        }

        public float GetBoundaryDistance(Vector3 direction)
        {
            return GetSizeIn(direction);
        }

        protected override void OnProjectileDisabled()
        {
            _container = null;
        }

        protected override ImpactResult GetImpactResult(
            Collider other, List<HitBox> hits)
        {
            if (other.TryGetComponent<HitBox>(out var hitBox)
                && !IsEnemy(hitBox))
                return ImpactResult.Ignore;

            var ownColliderCenter = GetColliderCenter();

            var distanceFromLastFrame = DistanceFromLastFrame;
            var backward = -transform.forward;

            var origin = ownColliderCenter + backward * distanceFromLastFrame;

            var radius = _collider.radius;
            if (!Physics.SphereCast(
                origin,
                radius - .3f,
                transform.forward,
                out var hitInfo,
                distanceFromLastFrame + radius))
                return ImpactResult.Ignore;

            var contactPoint = hitInfo.point + backward * 0.1f;

            var explosionhitsCount = Physics.OverlapSphereNonAlloc(
                 transform.position,
                 _aoeRadius,
                 _explosionOverlapBuffer,
                 LayerMask.GetMask("HitBox"));

            for (int i = 0; i < explosionhitsCount; i++)
            {
                var hit = _explosionOverlapBuffer[i];

                if (BehindObstacle(hit, contactPoint))
                    continue;

                hitBox = hit.GetComponent<HitBox>();

                if (!IsEnemy(hitBox))
                    continue;

                hits.Add(hitBox);
            }

            return ImpactResult.Consume;
        }

        protected override Vector3 GetColliderCenter()
        {
            return transform.TransformPoint(_collider.center);
        }

        private bool BehindObstacle(Collider target, Vector3 origin)
        {
            var hitColliderCenter = target switch
            {
                BoxCollider box => target.transform.TransformPoint(box.center),
                SphereCollider sp => target.transform.TransformPoint(sp.center),
                CapsuleCollider capsule => target.transform.TransformPoint(capsule.center),
                _ => target.bounds.center
            };

            var axis = hitColliderCenter - origin;
            var distance = axis.magnitude;
            var direction = axis / distance;

            var ray = new Ray(origin, direction);
            var hitsCount = Physics.RaycastNonAlloc(ray, _rayBuffer, distance);

            Debug.DrawRay(origin, axis, Color.red, 100f);

            for (int i = 0; i < hitsCount; i++)
            {
                var hit = _rayBuffer[i].collider;

                if (hit == target)
                    continue;

                if (hit.isTrigger)
                    continue;

                if (hit.TryGetComponent<HitBox>(out _))
                    continue;

                if (hit.transform.IsChildOf(transform))
                    continue;

                return true;
            }

            return false;
        }
    }
}
