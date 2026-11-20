using Assets.Scripts.Timers;
using System;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    [Serializable]
    public class AnimationController
    {
        private const float MinSpeedMultiplier = 0.1f;
        private const float MinDuration = 0.01f;

        [SerializeField] private string _parameterName;
        [SerializeField] private AnimationConfig[] _animationSequence;
        [SerializeField] private AnimationEventListener _animationEventListener;

        private Animator _animator;
        private Timer _timer;

        private int _parameterHash;
        private AnimationConfig _keyConfig;

        private float _beforeSpeed;
        private float _afterSpeed;

        private float _requestedDuration;
        private float _animationTimeError;

        private bool _isStarted;

        public void Initialize(Animator animator)
        {
            _animator = animator;

            _parameterHash = Animator.StringToHash(_parameterName);

            int keyCount = 0;

            foreach (var config in _animationSequence)
            {
                config.Initialize();
                if (config.IsKey)
                {
                    _keyConfig = config;
                    keyCount++;
                }
            }

            if (keyCount != 1)
            {
                Debug.LogError(
                    $"Expected exactly one key clip. Found: {keyCount}");
            }

            _timer = new Timer(0f);

            //_animationEventListener.EventPerformed += OnKeyPointReached;
        }

        public void Update(float deltaTime)
        {
            _timer?.Tick(deltaTime);
        }

        public void Play(float keyFrameTime)
        {
            _isStarted = true;

            var animationError = GetRemainingAnimationTime();

            _requestedDuration = keyFrameTime;

            var targetKeyTime = Mathf.Max(
                keyFrameTime - animationError,
                MinDuration);

            CalculateDurations(
                out var beforeKeyDuration,
                out var afterKeyDuration);

            CalculateSpeeds(
                targetKeyTime,
                beforeKeyDuration,
                afterKeyDuration);

            ApplySpeedMultipliers();

            var actualBeforeDuration =
                beforeKeyDuration / _beforeSpeed;

            var actualAfterDuration =
                afterKeyDuration / _afterSpeed;

            var totalDuration =
                actualBeforeDuration + actualAfterDuration;

            _timer.Restart(totalDuration);

            _animator.ResetTrigger(_parameterHash);
            _animator.SetTrigger(_parameterHash);
        }

        public void OnKeyPointReached()
        {
            if (!_isStarted)
                return;

            _animationTimeError = _timer.RemainingTime;

            _animator.SetFloat(
                _keyConfig.SpeedMultiplierHash,
                _afterSpeed);
        }

        private float GetRemainingAnimationTime()
        {
            var state = _animator.GetCurrentAnimatorStateInfo(1);

            var normalizedRemaining =
                Mathf.Max(0f, 1f - state.normalizedTime);

            var effectiveSpeed =
                _animator.speed *
                state.speed *
                state.speedMultiplier;

            if (effectiveSpeed <= 0f)
                return 0f;

            return state.length *
                   normalizedRemaining /
                   effectiveSpeed;
        }

        private void CalculateDurations(
            out float beforeKeyDuration,
            out float afterKeyDuration)
        {
            beforeKeyDuration = 0f;
            afterKeyDuration = 0f;

            bool keyFound = false;

            foreach (var config in _animationSequence)
            {
                if (!keyFound)
                {
                    if (config.IsKey)
                    {
                        beforeKeyDuration += config.KeyPoint;

                        afterKeyDuration +=
                            config.Length - config.KeyPoint;

                        keyFound = true;
                    }
                    else
                    {
                        beforeKeyDuration += config.Length;
                    }
                }
                else
                {
                    afterKeyDuration += config.Length;
                }
            }
        }

        private void CalculateSpeeds(
            float targetKeyTime,
            float beforeKeyDuration,
            float afterKeyDuration)
        {
            if (targetKeyTime <= 0f ||
                beforeKeyDuration <= 0f)
            {
                _beforeSpeed = MinSpeedMultiplier;
            }
            else
            {
                _beforeSpeed = Mathf.Max(
                    beforeKeyDuration / targetKeyTime,
                    MinSpeedMultiplier);
            }

            var afterTargetDuration =
                Mathf.Max(targetKeyTime * 0.5f, MinDuration);

            if (afterKeyDuration <= 0f)
            {
                _afterSpeed = MinSpeedMultiplier;
            }
            else
            {
                _afterSpeed = Mathf.Max(
                    afterKeyDuration / afterTargetDuration,
                    MinSpeedMultiplier);
            }
        }

        private void ApplySpeedMultipliers()
        {
            bool keyFound = false;

            foreach (var config in _animationSequence)
            {
                if (config.IsKey)
                {
                    keyFound = true;

                    _animator.SetFloat(
                        config.SpeedMultiplierHash,
                        _beforeSpeed);
                }
                else
                {
                    _animator.SetFloat(
                        config.SpeedMultiplierHash,
                        keyFound ? _afterSpeed : _beforeSpeed);
                }
            }
        }
    }
}
