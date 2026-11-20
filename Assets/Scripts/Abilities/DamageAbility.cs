using Assets.Scripts.Projectiles;
using System;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [Serializable]
    public class DamageAbility : Ability<Vector3>
    {
        [SerializeField] private float _damage;
        [SerializeField] private Transform _projectileSpawnPosition;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private Collider _collider;

        protected override void Execute(Vector3 targetPosition)
        {
            var spawnPosition = _projectileSpawnPosition.transform.position;
            var spawnOrientation = Quaternion.LookRotation(
                targetPosition
                - spawnPosition);

            var projectile = GameObject.Instantiate(
                 _projectile,
                 spawnPosition,
                 spawnOrientation);

            projectile.Initialize(_damage, Allie.Hero, _collider);
            projectile.Launch();
        }
    }
}
