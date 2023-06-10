using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Vector3> spawnPoints;
    [SerializeField] private float spawnRadius;

    private List<(Vector3, float)> spawnAreas;

    private void Awake()
    {
        foreach (Vector3 point in spawnPoints)
        {
            spawnAreas.Add((point, spawnRadius));
        }
    }

    private void Start()
    {
        
    }

    private void SpawnEnemy()
    {

    }

}
