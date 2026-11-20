using Assets.Scripts.Animation;
using Assets.Scripts.Heroes.Control;
using Assets.Scripts.Timers;
using System;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    [Serializable]
    public abstract class BossCombatMove
    {
        [SerializeField] private Timer _speed;
        [SerializeField] private Timer _cooldown;
        [SerializeField] private AnimationController _animationController;

        private BossTargetFinder _targetFinder;

        private HeroTarget _target;

        protected bool IsReady => _cooldown.IsReady;

        public event Action Performed;

        public void Initialize(
            Animator animator,
            BossTargetFinder targetFinder)
        {
            _targetFinder = targetFinder;

            _animationController.Initialize(animator);

            _speed.Expired += OnActionPerformed;
        }

        public void Update(float deltaTime)
        {
            _cooldown.Tick(deltaTime);
            _speed.Tick(deltaTime);
            _animationController.Update(deltaTime);

            OnUpdate();
        }

        public bool CanPerform()
        {
            return _targetFinder.TryGetCurrent(out _target)
                && _cooldown.IsReady
                && OnCanPerform();
        }

        public void TryPerform(
            HeroTarget target)
        {
            _target = target;

            _cooldown.Trigger();
            _speed.Trigger();
            _animationController.Play(
                _speed.Duration);

            OnPerform();
        }

        private void OnActionPerformed()
        {
            OnPerformed(_target.Hero);
            Performed?.Invoke();
        }

        protected bool TryGetCurrentTarget(out HeroController target)
        {
            if (_targetFinder.TryGetCurrent(out var heroTarget))
            {
                target = heroTarget.Hero;
                return true;
            }

            target = null;
            return false;
        }

        protected HeroController GetSavedTarget()
        {
            return _target.Hero;
        }

        protected virtual void OnPerform() { }

        protected virtual void OnUpdate() { }

        protected abstract void OnPerformed(
            HeroController target);

        protected abstract bool OnCanPerform();
    }
}
