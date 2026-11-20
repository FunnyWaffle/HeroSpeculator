using Assets.Scripts.Extensions;
using Assets.Scripts.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SpawnZones
{
    public class RockSpawnZone : SpawnZone<Rock>
    {
        private readonly List<Rock> _reserved = new();

        public bool TryReserveClosestRock(Vector3 sourcePosition, out Rock rock)
        {
            rock = Objects.SelectClosest(
                sourcePosition,
                r => !_reserved.Contains(r));

            if (rock == null)
                return false;

            _reserved.Add(rock);

            return true;
        }

        protected override void InitializeObject(Rock rock)
        {

        }

        protected override void OnObjectRespawned(Rock rock)
        {
            if (_reserved.Contains(rock))
                _reserved.Remove(rock);
        }
    }
}
