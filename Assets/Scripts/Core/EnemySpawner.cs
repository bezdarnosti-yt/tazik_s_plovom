using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Спавн врагов")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private int maxEnemies = 50;
    [SerializeField] private LayerMask groundLayer = 1;
    
    private int currentEnemyCount = 0;
    
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / spawnRate);
            
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }
    
    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        
        currentEnemyCount++;
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 10f, randomCircle.y);
        
        RaycastHit hit;
        if (Physics.Raycast(spawnPos, Vector3.down, out hit, 20f, groundLayer))
        {
            spawnPos.y = hit.point.y + 1f;
        }
        else
        {
            spawnPos.y = player.position.y;
        }
        
        return spawnPos;
    }
    
    public void OnEnemyDied()
    {
        currentEnemyCount--;
    }
}