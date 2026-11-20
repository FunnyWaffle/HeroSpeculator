using System;
using UnityEngine;

namespace Assets.Scripts.Timers
{
    [Serializable]
    public class Timer
    {
        [SerializeField, Min(0f)] private float _duration;
        private float _remainingTime;

        private bool _isReadyFromPreviousFrame;
        private bool _isNotExpired;

        public Timer(float duration)
        {
            _duration = duration;
        }

        public bool IsReady => _remainingTime <= 0;
        public float Duration
        {
            get => _duration;
            set
            {
                if (!IsReady)
                    return;

                _duration = Mathf.Max(0f, value);
            }
        }
        public float RemainingTime => _remainingTime;

        public event Action Expired;

        public virtual void Trigger()
        {
            if (!_isReadyFromPreviousFrame)
                _remainingTime = 0f;

            _remainingTime += _duration;

            _isReadyFromPreviousFrame = false;
            _isNotExpired = true;
        }

        public virtual void Tick(float deltaTime)
        {
            _remainingTime -= deltaTime;

            if (_isReadyFromPreviousFrame)
            {
                _remainingTime = 0f;
                _isReadyFromPreviousFrame = false;
            }
            else if (IsReady && _isNotExpired)
            {
                _isReadyFromPreviousFrame = true;
                _isNotExpired = false;
                Expired?.Invoke();
            }
        }

        public void Stop()
        {
            _remainingTime = 0;

            _isReadyFromPreviousFrame = false;
            _isNotExpired = false;
        }

        public void Restart(float duration)
        {
            _duration = Mathf.Max(0f, duration);
            _remainingTime = duration;

            _isReadyFromPreviousFrame = false;
            _isNotExpired = true;
        }
    }
}
