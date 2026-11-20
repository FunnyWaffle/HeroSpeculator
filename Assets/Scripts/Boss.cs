using Assets.Scripts.Projectiles;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class Boss : MonoBehaviour, IDamageable
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Health _health;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Transform _hpBarPosition;
        [SerializeField] private HitBox _hitBox;
        [SerializeField] private Camera _camera;
        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshObstacle _deathObstacle;
        [SerializeField] private BossCombatController _combatController;

        private readonly int _forwardSpeedHash = Animator.StringToHash("Forward Speed");
        private readonly int _deathHash = Animator.StringToHash("Died");

        public bool IsDead => _health.Current <= 0;

        private void Start()
        {
            _combatController.Initialize(_health);
            _hitBox.Initialize(this, Allie.Boss);
        }

        private void Update()
        {
            if (IsDead)
                return;

            _animator.SetFloat(_forwardSpeedHash, _agent.velocity.magnitude);
        }

        private void LateUpdate()
        {
            if (IsDead)
                return;

            var screenPoint = _camera.WorldToScreenPoint(_hpBarPosition.position);
            _healthBar.SetPosition(screenPoint);
        }

        public void TakeDamage(float amount)
        {
            if (IsDead)
                return;

            Debug.Log($"Boss's HP reduced, current HP is {_health.Current}");

            _health.Reduce(amount);

            if (IsDead)
            {
                Die();
            }

            _healthBar.SetImageFillAmount(_health.Current / _health.Max);
        }

        private void Die()
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.enabled = false;

            _deathObstacle.enabled = true;

            _animator.SetTrigger(_deathHash);
        }
    }
}
