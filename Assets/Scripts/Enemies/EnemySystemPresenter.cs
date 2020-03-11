using System.Collections;
using EnemySystem.Views;
using Misc;
using UnityEngine;

public class EnemySystemPresenter : MonoBehaviour
{
    [SerializeField] 
    private EnemySystemView enemySystemView;
    [SerializeField] 
    private EnemyData[] enemyDatas;

    private const float spawnEachSeconds = 1;
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

    private void OnEnemyCompletePath(EnemyView obj)
    {
        _poolManager.GetPool(obj.EnemyData.enemyViewPrefab).ReleaseObject(obj);
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
        enemySystemView.StartEnemy(newEnemy, enemyData.MovingSpeed);
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
