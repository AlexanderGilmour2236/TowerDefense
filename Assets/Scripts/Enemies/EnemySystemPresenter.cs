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
    
    private const float spawnEachSeconds = 0.2f;
    private const float durationSeconds = 10;

    private PrefabPoolManager<EnemyView> _poolManager = new PrefabPoolManager<EnemyView>();
    private Coroutine _spawnCoroutine;

    public event Action<EnemyView> EnemyDie;
    public event Action<EnemyView> EnemyCompletePath;
    
    public void Init()
    {
        enemySystemView.EnemyCompletePath += OnEnemyCompletePath;
        enemySystemView.EnemyDied += OnEnemyDie;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        enemySystemView.EnemyCompletePath -= OnEnemyCompletePath;
        enemySystemView.EnemyDied -= OnEnemyDie;
        _poolManager.Clear();
        _poolManager = null;
    }

    public void StartWave()
    {
        _spawnCoroutine = StartCoroutine(SpawnCoroutine(durationSeconds, spawnEachSeconds));
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
        enemySystemView.StopEnemy(enemy);
        Enemies.Remove(enemy);
        _poolManager.GetPool(enemy.EnemyData.enemyViewPrefab).ReleaseObject(enemy);
    }

    public void ClearEnemies()
    {
        StopCoroutine(_spawnCoroutine);
        
        var count = Enemies.Count;
        for (var i = 0; i < count; i++)
        {
            DestroyEnemy(Enemies[0]);
        }
        
        Enemies.Clear();
        _poolManager.Clear();
    }
    
    private void OnEnemyCompletePath(EnemyView enemy)
    {
        DestroyEnemy(enemy);
        EnemyCompletePath?.Invoke(enemy);
    }
    
    private void OnEnemyDie(EnemyView enemy)
    {
        DestroyEnemy(enemy);
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
