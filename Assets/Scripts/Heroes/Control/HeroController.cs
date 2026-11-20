using Assets.Scripts.Heroes.HeroSpecializations;
using Assets.Scripts.NPCs;
using Assets.Scripts.Projectiles;
using Assets.Scripts.StatModificators.Buffs;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Heroes.Control
{
    public class HeroController : MonoBehaviour, IHasPosition, IDamageable, IMoveTarget
    {
        [SerializeField] private HeroMover _movement;
        [SerializeField] private Camera _camera;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private HitBox _hitBox;
        [SerializeField] private Health _health;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Transform _hpBarPosition;
        [SerializeField] private HeroSpecialization _specialization;
        [SerializeField] private Animator _animator;
        [SerializeField] private HeroRotator _rotator;
        [SerializeField] private ParticleSystem _healParticles;

        private readonly int _holdingShieldTriggerHash = Animator.StringToHash("IsHoldingShield");
        private readonly int _forwardSpeedHash = Animator.StringToHash("ForwardSpeed");

        private DefenceReductionBuff _defence;

        public float HealthPoints => _health.Current;
        public bool IsHpFull => _health.IsFull();
        public float Radius => _agent.radius;

        public Vector3 Position => transform.position;

        private void Awake()
        {
            _health.Initialize();
            _hitBox.Initialize(this, Allie.Hero);
            _movement.Initialize(_camera, _agent, transform);
        }

        private void OnEnable()
        {
            _movement.Enable();
        }

        private void OnDisable()
        {
            _movement.Disable();
        }

        private void Update()
        {
            _specialization.Use();
            _defence?.UpdateCooldown(Time.deltaTime);
            _animator.SetFloat(_forwardSpeedHash, _agent.velocity.magnitude);
        }

        private void LateUpdate()
        {
            var screenPoint = _camera.WorldToScreenPoint(_hpBarPosition.position);
            _healthBar.SetPosition(screenPoint);

            if (!_specialization.TryGetTarget(out var target))
                return;

            _rotator.Rotate(target, _agent.velocity, Time.deltaTime);
        }

        public void TakeDamage(float amount)
        {
            if (_defence != null)
                amount = _defence.Apply(amount);

            _health.Reduce(amount);

            var health = HealthPoints;

            if (health == 0)
                gameObject.SetActive(false);

            _healthBar.SetImageFillAmount(health / _health.Max);
        }

        public void TakeHeal(float amount)
        {
            _health.Restore(amount);

            _healthBar.SetImageFillAmount(HealthPoints / _health.Max);

            _healParticles.Play();
        }

        public void ApplyBuff(DefenceReductionBuff buff)
        {
            if (_defence != null)
                _defence.Expired -= CancelBuff;

            _defence = buff;
            _defence.Expired += CancelBuff;

            _animator.SetBool(_holdingShieldTriggerHash, true);
        }

        private void CancelBuff()
        {
            _defence.Expired -= CancelBuff;
            _defence = null;

            _animator.SetBool(_holdingShieldTriggerHash, false);
        }

        public float GetBoundaryDistance(Vector3 direction)
        {
            return _agent.radius;
        }
    }
}