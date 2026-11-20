using System;
using UnityEngine;

namespace Assets.Scripts.SpawnZones
{
    public abstract class Respawnable : MonoBehaviour
    {
        public event Action<Respawnable> Despawned;

        public void Despawn()
        {
            gameObject.SetActive(false);
            OnDisabled();
            Despawned?.Invoke(this);
        }

        public void Respawn(Vector3 position)
        {
            gameObject.SetActive(true);
            SetPosition(position);
        }

        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        protected virtual void OnDisabled() { }
    }
}
