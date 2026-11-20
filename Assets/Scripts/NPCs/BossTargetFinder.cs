using Assets.Scripts.Extensions;
using Assets.Scripts.Heroes.Control;
using System;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    [Serializable]
    public class BossTargetFinder
    {
        [SerializeField] private HeroController[] _heroes;

        private Transform _transform;

        private HeroTarget _target;
        private bool _overdue;

        public void Initialize(Transform transform)
        {
            _transform = transform;
        }

        public void Invalidate()
        {
            _overdue = true;
        }

        public bool TryGetCurrent(out HeroTarget target)
        {
            if (_target.Hero != null
                && !_overdue)
            {
                target = _target;
                return true;
            }

            if (TryGetClosestHero(out target))
            {
                _target = target;
                _overdue = false;
                return true;
            }

            _target = default;
            return false;
        }

        private bool TryGetClosestHero(out HeroTarget heroTarget)
        {
            var closestHero = _heroes.SelectClosest(
                 _transform.position,
                 out var position,
                 out var distance,
                 h => h.gameObject.activeSelf);

            if (closestHero == null)
            {
                heroTarget = default;
                return false;
            }

            heroTarget = new HeroTarget(closestHero, position, distance);
            return true;
        }
    }

    public readonly struct HeroTarget
    {
        public HeroController Hero { get; }
        public Vector3 Position { get; }
        public float Distance { get; }

        public HeroTarget(HeroController hero, Vector3 position, float distance)
        {
            Hero = hero;
            Position = position;
            Distance = distance;
        }
    }
}
