using System.Collections;
using System.Globalization;
using Pool;
using TMPro;
using UnityEngine;

public class CubeController : MonoBehaviour
{
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private int cubePoolStartStorage;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private TMP_InputField spawnInputField;
        [SerializeField] private TMP_InputField speedInputField;
        [SerializeField] private TMP_InputField distanceInputField;
        [SerializeField] private TMP_Text currentSpeedText;
        [SerializeField] private TMP_Text currentDistanceText;
        [SerializeField] private TMP_Text currentSpawnCooldownText;
        private CubePool _cubePool;
        private float _spawnCooldown = 1;
        private float _moveSpeed = 1;
        private float _moveDistance = 1;

        private void Start()
        {
            currentSpawnCooldownText.text = $"Current cooldown: {_spawnCooldown}";
            currentSpeedText.text = $"Current speed: {_moveSpeed}";
            currentDistanceText.text = $"Current distance: {_moveDistance}";
            _cubePool = new CubePool(cubePrefab, cubePoolStartStorage);
            spawnInputField.onEndEdit.AddListener(OnSpawnCooldownChangeHandler);
            speedInputField.onEndEdit.AddListener(OnSpeedChangeHandler);
            distanceInputField.onEndEdit.AddListener(OnDistanceChangeHandler);
            StartCoroutine(SpawnCube());
        }

        private void OnSpawnCooldownChangeHandler(string newValue)
        {
            if (InputFieldValueChecker(newValue))
            {
                _spawnCooldown = float.Parse(ReplaceToDot(newValue), CultureInfo.InvariantCulture);
                if (_spawnCooldown <= 0)
                {
                    _spawnCooldown = 0.1f;
                }
                spawnInputField.text = "";
                currentSpawnCooldownText.text = $"Current cooldown: {_spawnCooldown}";
            }
            else
            {
                spawnInputField.text = "";
            }
        }

        private void OnSpeedChangeHandler(string newValue)
        {
            
            if (InputFieldValueChecker(newValue))
            {
                _moveSpeed = float.Parse(ReplaceToDot(newValue), CultureInfo.InvariantCulture);
                if (_moveSpeed <= 0)
                {
                    _moveSpeed = 0.1f;
                }
                speedInputField.text = "";
                currentSpeedText.text = $"Current speed: {_moveSpeed}";
            }
            else
            {
                speedInputField.text = "";
            }
        }

        private void OnDistanceChangeHandler(string newValue)
        {
            if (InputFieldValueChecker(newValue))
            {
                _moveDistance = float.Parse(ReplaceToDot(newValue), CultureInfo.InvariantCulture);
                if (_moveDistance <= 0)
                {
                    _moveDistance = 0.1f;
                }
                distanceInputField.text = "";
                currentDistanceText.text = $"Current distance: {_moveDistance}";
            }
            else
            {
                distanceInputField.text = "";
            }
        }

        private bool InputFieldValueChecker(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return false;
            }
            foreach (var value in newValue.ToCharArray())
            {
                if (value >= '0' && value <= '9' || value == '.' || value == ',')
                {
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private string ReplaceToDot(string value)
        {
            var newValueChar = value.ToCharArray();
            for (int i = 0; i < newValueChar.Length; i++)
            {
                if (newValueChar[i] != ',') continue;
                newValueChar[i] = '.';
                break;
            }
            return new string(newValueChar);
        }
        
        private IEnumerator SpawnCube()
        {
            for (float i = 0; i < _spawnCooldown; i += Time.deltaTime)
            {
                yield return null;
            }
            SpawnCubeSetup();
        }

        private void SpawnCubeSetup()
        {
            var cubeComponent = _cubePool.Pop();
            cubeComponent.gameObject.transform.position = spawnPosition.position;
            cubeComponent.OnFinishMove += OnFinishCubeMoveHandler;
            cubeComponent.StartCube(_moveSpeed, _moveDistance);
            StartCoroutine(SpawnCube());
        }

        private void OnFinishCubeMoveHandler(Cube cubeComponent)
        {
            cubeComponent.OnFinishMove -= OnFinishCubeMoveHandler;
            _cubePool.Push(cubeComponent);
        }

        private void OnDestroy()
        {
            spawnInputField.onEndEdit.RemoveListener(OnSpawnCooldownChangeHandler);
            speedInputField.onEndEdit.RemoveListener(OnSpeedChangeHandler);
            distanceInputField.onEndEdit.RemoveListener(OnDistanceChangeHandler);
        }
}