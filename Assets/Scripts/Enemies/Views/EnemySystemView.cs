using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace EnemySystem.Views
{
    public class EnemySystemView : MonoBehaviour
    {
        [SerializeField] 
        private EnemyPath enemyPath;
        [SerializeField]
        private Transform enemyParent;
        
        private Vector3[] _enemyPathPoints;
        
        public Dictionary<EnemyView, Tweener> EnemyTweens { get; } = new Dictionary<EnemyView, Tweener>();
        public Transform EnemyParent => enemyParent;
        public float CompletePathPercent(EnemyView view) => EnemyTweens[view]?.ElapsedPercentage() ?? 0;
        
        public event Action<EnemyView> EnemyCompletePath;
        public event Action<EnemyView> EnemyDied;
        private void Start()
        {
            _enemyPathPoints = enemyPath.Points.Select(point => point.position).ToArray();
        }

        public void StartEnemy(EnemyView newEnemyView)
        {
            newEnemyView.transform.position = enemyPath.Points[0].position;
            newEnemyView.EnemyDie += OnEnemyDie;
            
            EnemyTweens.Add(newEnemyView,newEnemyView.transform.DOPath(_enemyPathPoints, newEnemyView.EnemyData.MovingSpeed)
                .SetEase(Ease.Linear)
                .SetSpeedBased(true)
                .SetLookAt(0)
                .OnComplete(
                () =>
                {
                    EnemyTweens.Remove(newEnemyView);
                    newEnemyView.EnemyDie -= OnEnemyDie;
                    EnemyCompletePath?.Invoke(newEnemyView);
                }));
        }

        public void StopEnemy(EnemyView enemyView)
        {
            if (!EnemyTweens.ContainsKey(enemyView)) return;
            
            var tween = EnemyTweens[enemyView];
            tween.Kill();
            EnemyTweens.Remove(enemyView);
        }
        
        private void OnEnemyDie(EnemyView enemyView)
        {
            enemyView.EnemyDie -= OnEnemyDie;
            StopEnemy(enemyView);

            EnemyDied?.Invoke(enemyView);
        }

        public void SetPath(EnemyPath enemyPath)
        {
            this.enemyPath = enemyPath;
            _enemyPathPoints = enemyPath.Points.Select(point => point.position).ToArray();
        }
    }
}