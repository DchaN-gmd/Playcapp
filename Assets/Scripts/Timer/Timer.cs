using System;
using UnityEngine;
using UnityEngine.Events;

namespace TimeSystem
{
    public class Timer : MonoBehaviour
    {
        #region Inspector fields
        [Header("Parameters")]
        [SerializeField] private bool _playOnStart;
        [SerializeField] [Min(0)] private float _startSeconds;

        [Header("Events")]
        [SerializeField] private UnityEvent _played;
        [SerializeField] private UnityEvent _stopped;
        [SerializeField] private UnityEvent<bool> _paused;
        #endregion

        #region Fields
        private float _seconds;
        private float _deltaTime;
        private float _deltaPauseTime;
        #endregion

        #region Properties
        public bool IsPlay { get; private set; }
        public bool IsPause { get; private set; }

        public float Seconds
        {
            get => _seconds;
            private set => _seconds = value < 0 ? 0 : value;
        }
        public float StartSeconds => _startSeconds;
        #endregion

        #region Events
        public event UnityAction Played
        {
            add => _played.AddListener(value);
            remove => _played.RemoveListener(value);
        }

        public event UnityAction Stopped
        {
            add => _stopped.AddListener(value);
            remove => _stopped.RemoveListener(value);
        }

        public event UnityAction<bool> Paused
        {
            add => _paused.AddListener(value);
            remove => _paused.RemoveListener(value);
        }
        #endregion

        private void Start()
        {
            if (_playOnStart) Play(_startSeconds);
        }

        private void Update()
        {
            if (!IsPlay || IsPause) return;

            Seconds = _startSeconds - (Time.time - _deltaTime);
            if (Seconds == 0) Stop();
        }

        public void Play(float seconds)
        {
            if (seconds < 0)
                throw new ArgumentException($"Seconds can't be less than 0. Seconds = {seconds}");

            if (IsPlay)
            {
                Debug.LogWarning("The timer is already running");
                return;
            }

            Seconds = seconds;
            _startSeconds = seconds;
            _deltaTime = Time.time;
            IsPlay = true;

            _played?.Invoke();
        }

        public void Stop()
        {
            if (!IsPlay)
            {
                Debug.LogWarning("The timer has already been stopped");
                return;
            }

            IsPlay = false;
            _stopped?.Invoke();
        }

        public void SetPause(bool value)
        {
            if (IsPause == value)
            {
                Debug.LogWarning(value ? "The timer is already on pause" : "The timer is already running");
                return;
            }

            IsPause = value;

            if (value) _deltaPauseTime = Time.time;
            else _deltaTime += Time.time - _deltaPauseTime;

            _paused?.Invoke(value);
        }
    }
}
