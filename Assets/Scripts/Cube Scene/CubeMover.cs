using System;
using UnityEngine;
using UnityEngine.Events;

namespace CubeScene
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeMover : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField] private bool _isDebug;

        [Header("Parameters")]
        [SerializeField] private bool _moveOnStart;
        [SerializeField] private bool _destroyOnStopMove;
        [SerializeField] private MoveAxis _moveAxis;
        [SerializeField] [Min(0)] private float _startSpeed;
        [SerializeField] [Min(0)] private float _startDistance;

        [Header("Events")]
        [SerializeField] private UnityEvent _startedMoving;
        [SerializeField] private UnityEvent _stopedMoving;
        #endregion

        #region Fields
        private Rigidbody _rigidbody;

        private float _speed;
        private float _startPostitionOnAxis;
        private float _distanceToDestroy;
        private float _movedDistance;

        private bool _isMove = false;
        #endregion

        #region Events
        public event UnityAction StartedMoving
        {
            add => _startedMoving?.AddListener(value);
            remove => _startedMoving?.RemoveListener(value);
        }

        public event UnityAction StoppedMoving
        {
            add => _stopedMoving?.AddListener(value);
            remove => _stopedMoving?.RemoveListener(value);
        }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (_moveOnStart) StartMove(_startSpeed, _startDistance);
        }

        private void FixedUpdate()
        {
            if (_isMove) Move();
        }
        #endregion

        #region Move Functions
        public void StartMove(float speed, float distanceToDestroy)
        {
            if (speed < 0) Debug.LogWarning("Speed can't be less then 0");
            if (distanceToDestroy < 0) Debug.LogWarning("Distance can't be less then 0");

            _speed = speed;
            _distanceToDestroy = distanceToDestroy;

            StartMove();
        }

        public void StartMove()
        {
            if (_isDebug) Debug.Log("Start Move");

            switch (_moveAxis)
            {
                case MoveAxis.X: _startPostitionOnAxis = transform.position.x; break;
                case MoveAxis.Y: _startPostitionOnAxis = transform.position.y; break;
                case MoveAxis.Z: _startPostitionOnAxis = transform.position.z; break;
                default: throw new ArgumentOutOfRangeException();
            }

            _isMove = true;
            _startedMoving?.Invoke();
        }

        private void StopMove()
        {
            if (_isDebug) Debug.Log("Stop Move");

            _rigidbody.velocity = Vector3.zero;
            _isMove = false;
            _stopedMoving?.Invoke();

            if (_destroyOnStopMove) Destroy(gameObject);
        }

        private void Move()
        {
            if (_isDebug) Debug.Log("Moving");
            
            MoveOnAxis();

            if (_movedDistance >= _distanceToDestroy) StopMove();
        }

        private void MoveOnAxis()
        {
            switch (_moveAxis)
            {
                case MoveAxis.X:
                    {
                        _rigidbody.velocity = Vector3.right * _speed;
                        _movedDistance = Mathf.Abs(transform.position.x - _startPostitionOnAxis);
                        break;
                    }
                case MoveAxis.Y:
                    {
                        _rigidbody.velocity = Vector3.up * _speed;
                        _movedDistance = Mathf.Abs(transform.position.y - _startPostitionOnAxis);
                        break;
                    }
                case MoveAxis.Z:
                    {
                        _rigidbody.velocity = Vector3.forward * _speed;
                        _movedDistance = Mathf.Abs(transform.position.z - _startPostitionOnAxis);
                        break;
                    }
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        private enum MoveAxis
        {
            X,
            Y,
            Z
        }
    }
}
