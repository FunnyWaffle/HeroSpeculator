using UnityEngine;

namespace Assets.Scripts.SpawnZones
{
    public class VoidZoneSpawner : SpawnZone<VoidZone>
    {
        [SerializeField] private Vector2 _zoneLifeTimeRange;

        protected override void InitializeObject(VoidZone zone)
        {
            zone.SetLifeTime(
                Random.Range(
                    _zoneLifeTimeRange.x,
                    _zoneLifeTimeRange.y));
            zone.Activate();
        }

        protected override void OnObjectRespawned(VoidZone zone)
        {
            zone.SetLifeTime(
                Random.Range(
                    _zoneLifeTimeRange.x,
                    _zoneLifeTimeRange.y));
            zone.Activate();
        }
    }
}
