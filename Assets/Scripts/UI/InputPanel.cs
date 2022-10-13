using CubeScene;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class InputPanel : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField] private bool _isDebug;

        [Header("Parameters")]
        [SerializeField] private bool _setParametersOnAwake;

        [Header("Controllers")]
        [SerializeField] private CubeSpawner _cubeSpawner;
        [SerializeField] private Button _enterButton;

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField _speedField;
        [SerializeField] private TMP_InputField _distanceField;
        [SerializeField] private TMP_InputField _timeToSpawnField;
        #endregion

        #region Fields
        private float _speed;
        private float _distance;
        private float _timeToSpawn;
        #endregion

        #region Unity Functions 
        private void Awake()
        {
            ResetParameters();
        }

        private void OnEnable()
        {
            _enterButton.onClick.AddListener(ResetParameters);
        }

        private void OnDisable()
        {
            _enterButton.onClick.RemoveListener(ResetParameters);
        }
        #endregion

        private void ResetParameters()
        {
            if (!float.TryParse(_speedField.text, out _speed))
            {
                Debug.LogWarning($"{_speedField.name} is not a number");
                _speedField.text = null;
                return;
            }

            if (!float.TryParse(_distanceField.text, out _distance))
            {
                Debug.LogWarning($"{_distanceField.name} is not a number");
                _distanceField.text = null;
                return;
            }

            if (!float.TryParse(_timeToSpawnField.text, out _timeToSpawn))
            {
                Debug.LogWarning($"{_timeToSpawnField.name} is not a number");
                _timeToSpawnField.text = null;
                return;
            }

            if (_isDebug)
            {
                Debug.Log($"Speed is {_speedField.text}");
                Debug.Log($"Distance is {_distanceField.text}");
                Debug.Log($"Time to spawn is {_timeToSpawnField.text}");
            }

            _cubeSpawner.SetSpawnParameters(_speed, _distance, _timeToSpawn);
        }
    }
}
