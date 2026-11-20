using Assets.Scripts.Projectiles;
using Assets.Scripts.SpawnZones;
using Assets.Scripts.Timers;
using UnityEngine;

namespace Assets.Scripts
{
    public class VoidZone : Respawnable
    {
        [SerializeField] private float _damagePerSecond = 10f;
        [SerializeField] private Timer _liveTimer;
        [SerializeField] private ParticleSystem _rootParticles;
        [SerializeField] private ParticleSystem[] _particlesToChangeDuration;
        [SerializeField] private ParticleSystem[] _particlesToChangeLifeTime;

        private void OnEnable()
        {
            _liveTimer.Expired += OnTimerExpired;
        }

        private void OnDisable()
        {
            _liveTimer.Expired -= OnTimerExpired;
        }

        private void Update()
        {
            _liveTimer.Tick(Time.deltaTime);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent<HitBox>(out var hitBox))
                return;

            hitBox.TryTakeHit(
                _damagePerSecond * Time.deltaTime,
                Allie.Boss);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _liveTimer.Trigger();
            _rootParticles.Play();
        }

        public void SetLifeTime(float time)
        {
            _liveTimer.Restart(time);

            foreach (var particles in _particlesToChangeDuration)
            {
                var main = particles.main;
                main.duration = _liveTimer.Duration;
            }

            foreach (var particles in _particlesToChangeLifeTime)
            {
                var main = particles.main;
                main.startLifetime = _liveTimer.Duration;
            }
        }

        private void OnTimerExpired()
        {
            gameObject.SetActive(false);
        }
    }
}
