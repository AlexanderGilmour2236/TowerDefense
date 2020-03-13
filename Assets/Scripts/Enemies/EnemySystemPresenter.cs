using System;
using System.Collections;
using System.Collections.Generic;
using EnemySystem;
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

    private PrefabPoolManager<EnemyView> _poolManager = new PrefabPoolManager<EnemyView>();
    private Coroutine _spawnCoroutine;
    private int _enemyWaveItemIndex;
    private int _enemyWaveItemSpawnedCount;
    private bool _spawnComplete;

    public event Action<EnemyView> EnemyDie;
    public event Action<EnemyView> EnemyCompletePath;
    public event Action WaveComplete; 
    
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

    public void StartWave(EnemyWave enemyWave)
    {
        _enemyWaveItemIndex = 0;
        _spawnCoroutine = StartCoroutine(SpawnCoroutine(enemyWave));
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
        CheckWaveComplete();
        EnemyCompletePath?.Invoke(enemy);
    }

    private void OnEnemyDie(EnemyView enemy)
    {
        DestroyEnemy(enemy);
        EnemyDie?.Invoke(enemy);
        CheckWaveComplete();
    }
    
    private void CheckWaveComplete()
    {
        if(_spawnComplete && Enemies.Count == 0)
            WaveComplete?.Invoke();
    }

    private IEnumerator SpawnCoroutine(EnemyWave enemyWave)
    {
        var timeLeft = enemyWave.Duration;
        _enemyWaveItemSpawnedCount = 0;
        _enemyWaveItemIndex = 0;
        _spawnComplete = false;
        
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(enemyWave.SpawnEach);
            timeLeft -= enemyWave.SpawnEach;

            var spawnRandom = enemyWave.WaveItems.Length <= _enemyWaveItemIndex 
                              || enemyWave.WaveItems[_enemyWaveItemIndex].EnemyData == null;
            
            // Spawn random enemy if there is no EnemyData in WaveItem
            SpawnEnemy(!spawnRandom ?
                enemyWave.WaveItems[_enemyWaveItemIndex].EnemyData 
                : enemyDatas[Random.Range(0,enemyDatas.Length)]);

            _enemyWaveItemSpawnedCount++;

            if (enemyWave.WaveItems.Length > _enemyWaveItemIndex && enemyWave.WaveItems[_enemyWaveItemIndex].Count <= _enemyWaveItemSpawnedCount)
            {
                _enemyWaveItemIndex++;
                _enemyWaveItemSpawnedCount = 0;
            }
        }
        _spawnComplete = true;
    }
}
