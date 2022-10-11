using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class CubePool
    {
        private readonly GameObject _cubePrefab;
        private readonly Stack<Cube> _storage;
        private readonly Transform _rooTransform;

        public CubePool(GameObject cubePrefab, int storageAmount)
        {
            _cubePrefab = cubePrefab;
            _storage = new Stack<Cube>(storageAmount);
            _rooTransform = new GameObject("PoolRoot").GetComponent<Transform>();
            FillPool(storageAmount);
        }

        public void Push(Cube cube)
        {
            cube.gameObject.SetActive(false);
            _storage.Push(cube);
        }

        public Cube Pop()
        {
            Cube cube;
            if (_storage.Count > 0)
            {
                cube = _storage.Pop();
                cube.gameObject.SetActive(true);
            }
            else
            {
                cube = Create();
            }

            return cube;
        }

        private Cube Create()
        {
            var cubeGameObject = Object.Instantiate(_cubePrefab, _rooTransform);
            var cube = cubeGameObject.GetComponent<Cube>();
            return cube;
        }

        private void FillPool(int storageAmount)
        {
            for (int i = 0; i < storageAmount; i++)
            {
                var cube = Create();
                cube.gameObject.SetActive(false);
                Push(cube); 
            }
        }
    }
}