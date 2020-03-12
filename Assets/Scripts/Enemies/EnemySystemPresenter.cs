using System;
using System.Collections;
using System.Collections.Generic;
using EnemySystem.Views;
using Misc;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySystemPresenter : MonoBehaviour
{
    [SerializeField] 
    private EnemySystemView enemySystemView;
    [SerializeField] 
    private EnemyData[] enemyDatas;
    
    public List<EnemyView> Enemies { get; } = new List<EnemyView>();
    public float CompletePathPercent(EnemyView view) => enemySystemView.CompletePathPercent(view);
    
    private const float spawnEachSeconds = 1f;
    private const float durationSeconds = 10;

    private PrefabPoolManager<EnemyView> _poolManager = new PrefabPoolManager<EnemyView>();
    
    public event Action<EnemyView> EnemyDie;
    
    public void Init()
    {
        StartWave();
        enemySystemView.EnemyCompletePath += DestroyEnemy;
        enemySystemView.EnemyDied += DestroyEnemy;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        enemySystemView.EnemyCompletePath -= DestroyEnemy;
        enemySystemView.EnemyDied -= DestroyEnemy;
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
        enemySystemView.StartEnemy(newEnemy);
    }
    
    private void DestroyEnemy(EnemyView enemy)
    {
        Enemies.Remove(enemy);
        _poolManager.GetPool(enemy.EnemyData.enemyViewPrefab).ReleaseObject(enemy);
        EnemyDie?.Invoke(enemy);
    }

    private IEnumerator SpawnCoroutine(float duration, float spawnInterval)
    {
        var timeLeft = duration;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            timeLeft -= spawnInterval;
            SpawnEnemy(enemyDatas[Random.Range(0,enemyDatas.Length)]);
        }
    }


}
