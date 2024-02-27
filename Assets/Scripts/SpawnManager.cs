using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using TMPro.EditorUtilities;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _tripleShotPowerUpPrefab;
    [SerializeField]
    private bool _StopSpawning = false;
    [SerializeField]
    private GameObject[] powerups;
    //Code for spawn enemy waves
    private List<GameObject> _aliveEnemies;

    private UIManager _UIManager;

    public int hazardCount = 3; // Number of enemies per wave
    public float spawnWait = 0.5f; // Time between enemy spawns
    public float startWait = 1; // Initial delay before spawning starts
    public float waveWait = 4; // Time between waves
    public Transform[] spawnPoints; // Array of spawn points

    // Start is called before the first frame update
    void Start()
    {
        _UIManager = FindAnyObjectByType<UIManager>();
        if (!_UIManager)
            Debug.LogError("UI Manager is Null");
        _aliveEnemies = new List<GameObject>();
        
    }


    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            // Pick a random spawn point for this wave
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            for (int i = 0; i < hazardCount; i++)
            {
                // Instantiate an enemy at the chosen spawn point
                Instantiate(_enemyPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
                yield return new WaitForSeconds(spawnWait);
            }

            // Wait before starting the next wave
            yield return new WaitForSeconds(waveWait);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    //spawn game objects every 5 seconds
    //Create a coroutine of type IENumerator
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_StopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_StopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 7);

            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }
    public void StartSpawning()
    {
        //Debug.Log("Start Spawning");
        StartCoroutine(SpawnWaves());
        StartCoroutine(SpawnPowerupRoutine());
    }
    public void OnPlayerDeath()
    {

        _StopSpawning = true;
    }
}
