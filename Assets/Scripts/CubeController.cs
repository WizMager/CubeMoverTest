using System.Collections;
using Pool;
using UnityEngine;
using UnityEngine.UI;

public class CubeController : MonoBehaviour
{
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private int cubePoolStartStorage;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private Slider spawnSlider;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private Slider distanceSlider;
        private CubePool _cubePool;
        private float _spawnCooldown;
        private float _moveSpeed;
        private float _moveDistance;

        private void Start()
        {
            _cubePool = new CubePool(cubePrefab, cubePoolStartStorage);
            _spawnCooldown = spawnSlider.value;
            _moveSpeed = speedSlider.value;
            _moveDistance = distanceSlider.value;
            spawnSlider.onValueChanged.AddListener(OnSpawnCooldownChangeHandler);
            speedSlider.onValueChanged.AddListener(OnSpeedChangeHandler);
            distanceSlider.onValueChanged.AddListener(OnDistanceChangeHandler);
            StartCoroutine(SpawnCube());
        }

        private void OnSpawnCooldownChangeHandler(float newValue)
        {
            _spawnCooldown = newValue;
        }

        private void OnSpeedChangeHandler(float newValue)
        {
            _moveSpeed = newValue;
        }

        private void OnDistanceChangeHandler(float newValue)
        {
            _moveDistance = newValue;
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
}