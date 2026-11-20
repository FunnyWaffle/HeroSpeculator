using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.SpawnZones
{
    public abstract class SpawnZone<T> : MonoBehaviour
        where T : Respawnable
    {
        [SerializeField] private Transform _startZoneCorner;
        [SerializeField] private Transform _endZoneCorner;

        [SerializeField] private T _prefab;

        [SerializeField] private float _maxObjects;

        private readonly List<T> _objects = new();

        protected IReadOnlyList<T> Objects => _objects;

        private void Awake()
        {
            for (int i = 0; i < _maxObjects; i++)
            {
                var @object = Instantiate(_prefab);
                var position = GetRandomPosition();
                @object.SetPosition(position);

                InitializeObject(@object);

                @object.Despawned += RespawnObject;

                _objects.Add(@object);
            }
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 start = _startZoneCorner.position;
            Vector3 end = _endZoneCorner.position;

            var position = new Vector3(
                Random.Range(start.x, end.x),
                Random.Range(start.y, end.y),
                Random.Range(start.z, end.z));

            return position;
        }

        protected abstract void InitializeObject(T @object);

        private void RespawnObject(Respawnable sender)
        {
            var position = GetRandomPosition();
            sender.Respawn(position);
            OnObjectRespawned((T)sender);
        }

        protected abstract void OnObjectRespawned(T @object);
    }
}
