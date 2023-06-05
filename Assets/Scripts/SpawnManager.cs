using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    //spawn game objects every 5 seconds
    //Create a coroutine of type IENumerator
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_StopSpawning == false) 
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7,0);
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
            int randomPowerUp = Random.Range(0,3);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
                }
    }
    public void StartSpawning()
    {
        //Debug.Log("Start Spawning");
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    public void OnPlayerDeath()
    {
        
        _StopSpawning = true;
    }
}
