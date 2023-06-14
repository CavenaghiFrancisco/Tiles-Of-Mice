using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int startingEnemyCount; //How much enemies will be instantiated at start
    [SerializeField] private int maxEnemyCount; //How much enemies will be instantiated in total

    [SerializeField] private List<Vector3> spawnPoints = new List<Vector3>(); //Marks the different points that are going to be shown
    [SerializeField] private bool randomSpawn = false; //Sets if the enemies should spawn at any random spawnpoint or just = amount on every point
    [SerializeField] private float maxSpawnRadius = 2; //Max radius of the circle that be used as spawner
    [SerializeField] private float respawnTime; //How much time will be between spawns
    
    private List<(Vector3, float)> spawnAreas = new List<(Vector3, float)>(); //Will create a list of possible areas using point and max radius
    private List<Enemy> enemyList = new List<Enemy>(); //Will save the enemies created
    //private bool canRespawn = true; //Sets if the enemy can be respawned
    private float timer = 0;//Timer that will be use to respawn
    private Transform enemyFolder; //Just a transform where are going to be placed the different created enemies. Just for order porpouses
    
    static List<List<Enemy>> enemyTypesList = new List<List<Enemy>>(); //A list with all the enemies of every types
    
    private void Awake()
    {
        foreach (Vector3 point in spawnPoints)
        {   //This will create a spawn area with the given radius starting from the spawn point
            float randomRadius = Random.Range(0.0f, maxSpawnRadius);
            spawnAreas.Add((point, randomRadius));
        }
    }

    private void Start()
    {
        CreateEnemyFolder();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>respawnTime)
        {   //Will spawn allways the timer arrives the respawn time
            SpawnEnemy(true);
            timer -= respawnTime;
        }
    }


    private void SpawnEnemy(bool randomArea, int index = 0)
    {
        (Vector3, float) spawnArea;
        if (spawnAreas.Count != 1)
        { 
            spawnArea = spawnAreas[randomArea ? Random.Range(0, spawnAreas.Count) : index];
        }
        else
        {
            spawnArea = spawnAreas[0];
        }


        //Will select a spawn area from the different
        Vector3 newPos = new Vector3(0,spawnArea.Item1.y,0);
        do
        {
            newPos.x = Random.Range(spawnArea.Item1.x - spawnArea.Item2, spawnArea.Item1.x + spawnArea.Item2);
            newPos.z = Random.Range(spawnArea.Item1.z - spawnArea.Item2, spawnArea.Item1.z + spawnArea.Item2);
        } while (Vector3.Distance(newPos,spawnArea.Item1)>spawnArea.Item2);

        Quaternion newRotation = Quaternion.identity;
        
        GameObject newEnemy = Instantiate(enemyPrefab, newPos, newRotation, enemyFolder);
        Enemy enemy = newEnemy.GetComponent<Enemy>();

        enemyList.Add(enemy);
    }

    private void CreateEnemyFolder()
    {
        GameObject go = Instantiate(new GameObject(), transform);
        go.name = enemyPrefab.name + "s";
        enemyFolder = go.transform;
    }

    private void OnDrawGizmos()
    {
        
        foreach ((Vector3,float) area in spawnAreas)
        {
            Gizmos.DrawSphere(area.Item1, area.Item2);
        }
    }
    private void OnValidate()
    {
    }

}
