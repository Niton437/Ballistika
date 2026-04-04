using System;
using System.Collections;
using UnityEngine;


namespace Targets
{
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject targetPrefab;

        [Header("Параметры области спавна")]
        [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f);
        [SerializeField] private float depthOffset = 0.5f;
        [SerializeField] private float spawnInterval;

        [Header("Параметры мишеней")]
        [SerializeField] private Vector3 startVelocity = new Vector3(5f, 0, 0);
        [SerializeField, Range(0.01f, 100f)] private float minMass;
        [SerializeField, Range(0.01f, 100f)] private float maxMass;
        [SerializeField, Range(0.01f, 2f)] private float minRadius;
        [SerializeField, Range(0.01f, 2f)] private float maxRadius;

        private void Awake()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                float randX = UnityEngine.Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f);
                float randY = UnityEngine.Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f);

                Vector3 spawnPosLocal = new Vector3(randX, randY, depthOffset);
                Vector3 spawnPosWorld = transform.TransformPoint(spawnPosLocal);

                GameObject newTarget = Instantiate(targetPrefab, spawnPosWorld, Quaternion.identity);

                float targetRadius = UnityEngine.Random.Range(minRadius, maxRadius);
                float targetMass = UnityEngine.Random.Range(minMass, maxMass);

                newTarget.transform.localScale = new Vector3(0.1f, targetRadius, targetRadius);

                Rigidbody targetRb = newTarget.GetComponent<Rigidbody>();
                if (targetRb != null)
                {
                    targetRb.mass = targetMass;
                    targetRb.useGravity = false;
                    targetRb.linearVelocity = startVelocity;
                }

                Destroy(newTarget, 15f);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Gizmos.matrix = transform.localToWorldMatrix;

            Vector3 cubeSize = new Vector3(areaSize.x, areaSize.y, 0.01f);
            Vector3 cubeCenter = new Vector3(0, 0, depthOffset);
            Gizmos.DrawWireCube(cubeCenter, cubeSize);

            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}