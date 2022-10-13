using TimeSystem;
using UnityEngine;

namespace CubeScene
{
    public class CubeSpawner : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField] private bool _isDebug;

        [Header("Controllers")]
        [SerializeField] private Timer _timer;

        [Header("Parameters")]
        [SerializeField] private CubeMover _cube;
        [SerializeField] private Transform _spawnPoint;
        #endregion

        #region Fields
        private float _cubeSpeed;
        private float _cubeTargetDistance;
        private float _timeToSpawn;

        private CubeMover _currentCube;
        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            _timer.Stopped += SpawnCube;
        }

        private void OnDisable()
        {
            _timer.Stopped -= SpawnCube;
        }

        private void Start()
        {
            SpawnCube();
        }
        #endregion

        public void SetSpawnParameters(float speed, float distance, float timeToSpawn)
        {
            if (_isDebug) Debug.Log("Set spawn parameters");

            if (speed < 0) Debug.LogWarning("Speed can't be less then 0");
            if (distance < 0) Debug.LogWarning("Distance can't be less then 0");
            if (timeToSpawn < 0) Debug.LogWarning("TimeToSpawn can't be less then 0");

            _cubeSpeed = speed;
            _cubeTargetDistance = distance;
            _timeToSpawn = timeToSpawn;
        }

        #region Spawn Functions
        private void SpawnCube()
        {
            if (_isDebug) Debug.Log("Spawn");

            _currentCube = Instantiate(_cube);

            _currentCube.transform.position = _spawnPoint.position;
            _currentCube.StartMove(_cubeSpeed, _cubeTargetDistance);
            _currentCube.StoppedMoving += OnCubeStopped;
        }

        private void OnCubeStopped()
        {
            if (_isDebug) Debug.Log("On cube stopped");

            _currentCube.StoppedMoving -= OnCubeStopped;
            _timer.Play(_timeToSpawn);
        }
        #endregion
    }
}
