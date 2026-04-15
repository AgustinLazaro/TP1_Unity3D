using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Config")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public int maxEnemies = 5;

    [Header("AI Injection")]
    public Transform playerTarget;
    public Transform[] patrolPoints;

    private int currentEnemies = 0;

    void Start()
    {
        if (spawnPoints.Length > 0 && enemyPrefab != null)
            StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies) SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, point.position, point.rotation);

        // Inject dependencies into the EnemyBrain
        if (enemy.TryGetComponent<EnemyBrain>(out EnemyBrain brain))
        {
            brain.target = playerTarget;
            if (patrolPoints.Length > 0) brain.patrolPoints = patrolPoints;
        }

        currentEnemies++;
    }

    public void OnEnemyDestroyed()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }
}