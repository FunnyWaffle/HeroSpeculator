using Assets.Scripts.SpawnZones;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    public class Projectile : Respawnable
    {
        [SerializeField] private float _speed;
        [SerializeField] private Collider _collider;

        private Collider _launcherCollider;
        private readonly List<HitBox> _hits = new();

        private float _damage;
        private Allie _allie;
        private bool _initialized;
        private bool _launched;

        protected Collider Collider => _collider;
        protected float DistanceFromLastFrame => _speed * Time.deltaTime;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            OnStarted();
        }

        private void Update()
        {
            if (_launched)
                transform.position += _speed * Time.deltaTime * transform.forward;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_launched)
                return;

            _hits.Clear();

            var result = GetImpactResult(other, _hits);

            if (result == ImpactResult.Ignore)
                return;

            foreach (var hit in _hits)
            {
                hit.TryTakeHit(_damage, _allie);
            }

            Despawn();
        }

        public void Launch()
        {
            _launched = true;
        }

        public void Initialize(float damage, Allie allie,
            Collider ignoredCollider)
        {
            if (_initialized)
                return;

            _damage = damage;
            _allie = allie;

            Physics.IgnoreCollision(_collider, ignoredCollider);
            _launcherCollider = ignoredCollider;

            _initialized = true;
        }

        public float GetSizeIn(Vector3 direction)
        {
            var bounds = _collider.bounds;
            var size = bounds.size;

            var maxSize = Mathf.Max(Mathf.Max(size.x, size.y), size.z);

            var closestPoint = _collider.ClosestPoint(direction * maxSize);

            var distance = Vector3.Distance(
                GetColliderCenter(),
                closestPoint);

            return distance;
        }

        protected virtual void OnStarted() { }

        protected virtual ImpactResult GetImpactResult(Collider other, List<HitBox> hits)
        {
            if (other.gameObject
                .TryGetComponent<HitBox>(out var hitBox))
            {
                if (!IsEnemy(hitBox))
                    return ImpactResult.Ignore;

                hits.Add(hitBox);
            }

            return ImpactResult.Consume;
        }

        protected sealed override void OnDisabled()
        {
            base.OnDisabled();
            ResetProjectile();
            OnProjectileDisabled();
        }

        protected virtual void OnProjectileDisabled() { }

        protected bool IsEnemy(HitBox hitBox)
        {
            return hitBox._allie != _allie;
        }

        private void ResetProjectile()
        {
            _launched = false;

            _damage = 0f;
            _allie = default;

            if (_launcherCollider != null)
                Physics.IgnoreCollision(_collider, _launcherCollider, false);

            _launcherCollider = null;
            _initialized = false;
        }

        protected virtual Vector3 GetColliderCenter()
        {
            return _collider.bounds.center;
        }
    }
}
