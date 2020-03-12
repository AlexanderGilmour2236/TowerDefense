using System.Collections;
using System.Collections.Generic;
using EnemySystem.Views;
using Misc;
using UnityEngine;

public class EnemySystemPresenter : MonoBehaviour
{
    [SerializeField] 
    private EnemySystemView enemySystemView;
    [SerializeField] 
    private EnemyData[] enemyDatas;
    
    public List<EnemyView> Enemies { get; } = new List<EnemyView>();
    public float CompletePathPercent(EnemyView view) => enemySystemView.CompletePathPercent(view);
    
    private const float spawnEachSeconds = 0.5f;
    private const int durationSeconds = 10;

    private PrefabPoolManager<EnemyView> _poolManager = new PrefabPoolManager<EnemyView>();
    
    public void Init()
    {
        StartWave();
        enemySystemView.EnemyCompletePath += OnEnemyCompletePath;
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
        enemySystemView.EnemyCompletePath -= OnEnemyCompletePath;
        _poolManager.Clear();
        _poolManager = null;
    }

    public void StartWave()
    {
        StartCoroutine(SpawnCoroutine(durationSeconds, spawnEachSeconds));
    }

    private void SpawnEnemy(EnemyData enemyData)
    {
        var enemyPool = _poolManager.GetPool(enemyData.enemyViewPrefab) ?? _poolManager.CreatePool(enemySystemView.EnemyParent, enemyData.enemyViewPrefab);

        var newEnemy = enemyPool.GetObject();
        newEnemy.SetData(enemyData);
        Enemies.Add(newEnemy);
        enemySystemView.StartEnemy(newEnemy, enemyData.MovingSpeed);
    }
    
    private void OnEnemyCompletePath(EnemyView enemy)
    {
        Enemies.Remove(enemy);
        _poolManager.GetPool(enemy.EnemyData.enemyViewPrefab).ReleaseObject(enemy);
    }

    private IEnumerator SpawnCoroutine(int duration, float spawnInterval)
    {
        var timeLeft = duration;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            timeLeft--;
            SpawnEnemy(enemyDatas[Random.Range(0,enemyDatas.Length)]);
        }
    }


}
