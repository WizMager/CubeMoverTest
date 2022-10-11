using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
        public Action<Cube> OnFinishMove;
        [SerializeField] private Transform cubeTransform;
        private float _moveSpeed;
        private float _moveDistance;

        public void StartCube(float moveSpeed, float moveDistance)
        {
                _moveSpeed = moveSpeed;
                _moveDistance = moveDistance;
                StartCoroutine(Move());
        }
        
        private IEnumerator Move()
        {
                for (float i = 0; i < _moveDistance; )
                {
                        var frameMoveDistance = Time.deltaTime * _moveSpeed;
                        i += frameMoveDistance;
                        cubeTransform.Translate(cubeTransform.forward * frameMoveDistance);
                        yield return null;
                }
                OnFinishMove?.Invoke(this);
        }
}