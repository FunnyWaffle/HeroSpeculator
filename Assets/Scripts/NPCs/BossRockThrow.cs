using Assets.Scripts.Heroes.Control;
using Assets.Scripts.Projectiles;
using Assets.Scripts.SpawnZones;
using System;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    [Serializable]
    public class BossRockThrow : BossCombatMove
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _pickUpRange;
        [SerializeField] private Transform _projectileSpawnPosition;
        [SerializeField] private RockSpawnZone _rockSpawnZone;

        private Transform _transform;
        private Collider _collider;
        private BossMover _mover;

        private Rock _rock;

        public void Initialize(
            Animator animator,
            Transform transform,
            BossMover mover,
            Collider collider,
            BossTargetFinder targetFinder)
        {
            Initialize(animator, targetFinder);

            _transform = transform;
            _mover = mover;
            _collider = collider;
        }

        protected override void OnUpdate()
        {
            if (IsReady
                && _rock == null
                && _rockSpawnZone.TryReserveClosestRock(
                    _transform.position,
                    out _rock))
            {
                _rock.Initialize(
                    _damage,
                    Allie.Boss,
                    _collider);

                _mover.TrySetTarget(_rock, BossTargetPriorities.Rock);
            }
        }

        protected override void OnPerform()
        {
            _rock.Take(_projectileSpawnPosition);

            _mover.SetRadius(3f);
        }

        protected override bool OnCanPerform() => CanPickupRock();

        protected override void OnPerformed(
            HeroController target)
        {
            var spawnPosition = _projectileSpawnPosition
                .transform.position;

            var direction = Vector3.Normalize(
                target.Position
                - spawnPosition);

            _rock.Throw(direction);
            _rock = null;

            _mover.SetRadius(1.5f);
            _mover.Stop();
        }

        private bool CanPickupRock()
        {
            if (_rock == null)
                return false;

            var axis = _rock.Position -
                _transform.position;

            var distance = Vector3.Magnitude(axis);
            var direction = Vector3.Normalize(axis);

            var pickRange =
                _rock.GetBoundaryDistance(direction)
                 + _mover.Radius
                 + _pickUpRange;

            return distance < pickRange;
        }
    }
}
